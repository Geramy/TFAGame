using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Reflection;
using OpenTK.Input;

using TFAGame.UI;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TFAGame
{
    public static class GameCore
    {
        public static bool UIPause = true;
        public static Size Size;
        public static Graphics graphics = null;
        public static GameGenerator gGen = null;
        public static string Username = "";
        private static CompositionContainer _container;
        public static PhysicsEngine PhysX = new PhysicsEngine();
        public const int PhysxFPS = 30;

        public static Dictionary<string, Player> players = new Dictionary<string,Player>();
        public static Memory CoreFiles = null;
        public static GameObject[] GrassPatches = null;
        public static GameObject CurrentItem = null;

        public static void SetupGCMode()
        {
            if (!Config.Configuration.Keys.Contains("GCMode"))
                return;
            switch ((string)Config.Configuration["GCMode"])
            {
                case "Batch":
                    System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Batch;
                    break;
                case "Interactive":
                    System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Interactive;
                    break;
                case "LowLatency":
                    System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;
                    break;
            }
        }
        public static void StartGame()
        {
            GameCore.NewPlayer(Username, graphics);
            GameCore.players[Username].AddObjects(gGen.GenerateStartGame(0, 0, 0));

            GameCore.NewPlayer("computer", graphics);
            GameCore.players["computer"].AddObjects(gGen.GenerateStartGame(1, Size.Width - 250, 0));
            UIPause = false;
        }

        public static void OnClickEvent(Element.ElementProperties prop)
        {
            switch ((string)prop.Value)
            {
                case "StartGame()":
                    StartGame();
                    break;
            }
        }

        public static void SetupModules()
        {
            #region Module Container Loading
            //System.ComponentModel.Composition.Primitives.ComposablePartCatalog
            List<ComposablePartCatalog> compose = new List<ComposablePartCatalog>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(assembly);
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "TFAGame.*.dll");
            #region Make Catalog
            compose.Add(assemblyCatalog);
            compose.Add(directoryCatalog);
            AggregateCatalog catalog = new AggregateCatalog(compose);
            #endregion Make Catalog
            _container = new CompositionContainer(catalog, true);

            try
            {

            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                StringBuilder error = new StringBuilder("Error(s) encountered loading extension modules. You may have an incompatible or out of date extension .dll in the current folder.");
                foreach (Exception loaderEx in ex.LoaderExceptions)
                    error.Append("\n " + loaderEx.Message);
            }

            #endregion Module Container Loading

            #region Module Loading
            Console.WriteLine("Loading Modules Initiated....");
            SetupCoreModules();

            SetupCoreEventModules();
            Console.WriteLine("Done loading modules.");
            #endregion Module Loading
            
        }
        public static void SetupCoreModules()
        {
            IEnumerable<Lazy<object, object>> exportEnumerable = _container.GetExports(typeof(CoreModule), null, null);
            foreach (Lazy<object, object> lazyExport in exportEnumerable)
            {
                IDictionary<string, object> metadata = (IDictionary<string, object>)lazyExport.Metadata;
                object nameObj;
                if (metadata.TryGetValue("Name", out nameObj))
                {
                    string name = (string)nameObj;
                    Console.WriteLine("Adding " + name + " Event Module.");
                    ModuleHandler.AddCoreModule(name, (CoreModule)lazyExport.Value);
                }
            }
            Console.WriteLine("Starting Core Modules.....");
            ModuleHandler.StartCoreModules();
            Console.WriteLine("Core Modules Started.....");
        }
        public static void SetupCoreEventModules()
        {
            Console.WriteLine("Loading modules...");
            IEnumerable<Lazy<object, object>> exportEnumerable = _container.GetExports(typeof(CoreEventModule), null, null);
            foreach (Lazy<object, object> lazyExport in exportEnumerable)
            {
                IDictionary<string, object> metadata = (IDictionary<string, object>)lazyExport.Metadata;
                object nameObj;
                if (metadata.TryGetValue("Name", out nameObj))
                {
                    string name = (string)nameObj;
                    Console.WriteLine("Adding " + name + " Core Event Module.");
                    ModuleHandler.AddCoreEventModule(name, (CoreEventModule)lazyExport.Value);
                }
            }
            Console.WriteLine("Starting Core Event Modules.....");
            ModuleHandler.StartCoreEventModules();
            Console.WriteLine("Core Event Modules Started.....");
        }

        public static void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

            }
        }

        public static void KeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

            }
        }

        public static void MouseDown(int x, int y)
        {   
            if (CurrentItem == null)
            {
                for (int i = 0; i < players.Keys.Count; i++)
                    CurrentItem = players[players.Keys.ToArray()[i]].GetObject(x, y, true);
                if (CurrentItem != null && !CurrentItem.Moveable)
                    CurrentItem = null;
            }
            else
            {
                GameObject secondItem = null;
                for (int i = 0; i < players.Keys.Count; i++)
                {
                    secondItem = players[players.Keys.ToArray()[i]].GetObject(x, y);
                    if (secondItem != null)
                        break;
                }
                if (secondItem == null)
                {
                    for (int i = 0; i < GameCore.GrassPatches.Length; i++)
                        if (GameCore.GrassPatches[i].MouseIsOver(x, y))
                            secondItem = GameCore.GrassPatches[i];
                }
                if (GameCore.PhysX.IsAttacking(ref CurrentItem))
                {
                    GameCore.PhysX.StopAttacking(ref CurrentItem);
                }
                if(secondItem == null)
                    PhysX.MoveTo(ref CurrentItem, ref secondItem, false);
                else
                    PhysX.MoveTo(ref CurrentItem, ref secondItem, true);
                CurrentItem = null;
            }
        }

        public static void NewPlayer(string Username, System.Drawing.Graphics graphics)
        {
            players.Add(Username, new Player(graphics));
        }

        public static void DrawPlayers()
        {
            
            foreach (string user in players.Keys)
                players[user].DrawObjects();
        }

        public static void DrawStatusBars()
        {
            foreach (string user in players.Keys)
                players[user].DrawStatusBars();
        }

        public static void DrawUI(int width, int height)
        {
            if (ModuleHandler._CoreModules.Keys.Contains("UITextures"))
            {
                CoreModule coreModule = null;
                ModuleHandler._CoreModules.TryGetValue("UITextures", out coreModule);
                UICore coreMod = (UICore)coreModule;
                coreMod.DrawUI(width, height);
            }
        }
    }
}
