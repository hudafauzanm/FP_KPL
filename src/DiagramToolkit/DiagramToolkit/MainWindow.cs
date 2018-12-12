﻿using DiagramToolkit.Commands;
using DiagramToolkit.MenuItems;
using DiagramToolkit.ToolbarItems;
using DiagramToolkit.Tools;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.IO;
using System.Reflection;

namespace DiagramToolkit
{
    public partial class MainWindow : Form
    {
        private IToolbox toolbox;
        private IEditor editor;
        private IToolbar toolbar;
        private IMenubar menubar;
        private IPlugin[] plugins;

        public MainWindow()
        {
            InitializeComponent();
            LoadPlugins();
            InitUI();
            
        }

        private void LoadPlugins()
        {
            //\\Mac\Home\git\diagram-toolkit\src\DiagramToolkit\DiagramToolkit\bin\Debug
            string path = Application.StartupPath;
            
            string[] pluginFiles = Directory.GetFiles(path, "*.DLL");
            plugins = new IPlugin[pluginFiles.Length];

            for (int i = 0; i < pluginFiles.Length; i++)
            {
                string args = pluginFiles[i].Substring(
                    pluginFiles[i].LastIndexOf("\\") + 1,
                    pluginFiles[i].IndexOf(".dll") -
                    pluginFiles[i].LastIndexOf("\\") - 1);

                Type type = null;

                try
                {
                    Assembly asm = Assembly.Load(args);

                    if (asm != null)
                    {
                        var pluginInterface = typeof(IPlugin);

                        Type[] types = asm.GetTypes();

                        foreach (Type t in types)
                        {
                            if (pluginInterface.IsAssignableFrom(t))
                                type = t;
                        }

                    }

                    if (type != null)
                    {
                        plugins[i] = (IPlugin)Activator.CreateInstance(type);
                        //plugins[i].Host = this;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             
            }
        }

        private void InitUI()
        {
            Debug.WriteLine("Initializing UI objects.");

            #region Editor and Canvas

            Debug.WriteLine("Loading canvas...");
            this.editor = new DefaultEditor();
            this.toolStripContainer1.ContentPanel.Controls.Add((Control)this.editor);

            ICanvas canvas1 = new DefaultCanvas();
            canvas1.Name = "Untitled-1";
            this.editor.AddCanvas(canvas1);

            ICanvas canvas2 = new DefaultCanvas();
            canvas2.Name = "Untitled-2";
            this.editor.AddCanvas(canvas2);

            #endregion

            #region Commands

            //BlackCanvasBgCommand blackCanvasBgCmd = new BlackCanvasBgCommand(this.canvas);
            //WhiteCanvasBgCommand whiteCanvasBgCmd = new WhiteCanvasBgCommand(this.canvas);

            #endregion

            #region Menubar

            Debug.WriteLine("Loading menubar...");
            this.menubar = new DefaultMenubar();
            this.Controls.Add((Control)this.menubar);

            DefaultMenuItem fileMenuItem = new DefaultMenuItem("File");
            this.menubar.AddMenuItem(fileMenuItem);

            DefaultMenuItem newMenuItem = new DefaultMenuItem("New");
            fileMenuItem.AddMenuItem(newMenuItem);
            fileMenuItem.AddSeparator();
            DefaultMenuItem exitMenuItem = new DefaultMenuItem("Exit");
            fileMenuItem.AddMenuItem(exitMenuItem);

            DefaultMenuItem editMenuItem = new DefaultMenuItem("Edit");
            this.menubar.AddMenuItem(editMenuItem);

            DefaultMenuItem undoMenuItem = new DefaultMenuItem("Undo");
            editMenuItem.AddMenuItem(undoMenuItem);
            DefaultMenuItem redoMenuItem = new DefaultMenuItem("Redo");
            editMenuItem.AddMenuItem(redoMenuItem);

            DefaultMenuItem viewMenuItem = new DefaultMenuItem("View");
            this.menubar.AddMenuItem(viewMenuItem);

            DefaultMenuItem helpMenuItem = new DefaultMenuItem("Help");
            this.menubar.AddMenuItem(helpMenuItem);

            DefaultMenuItem aboutMenuItem = new DefaultMenuItem("About");
            helpMenuItem.AddMenuItem(aboutMenuItem);

            #endregion

            #region Toolbox

            // Initializing toolbox
            Debug.WriteLine("Loading toolbox...");
            this.toolbox = new DefaultToolbox();
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add((Control)this.toolbox);
            this.editor.Toolbox = toolbox;

            #endregion

            #region Tools

            // Initializing tools
            Debug.WriteLine("Loading tools...");
            this.toolbox.AddTool(new SelectionTool());
            this.toolbox.AddSeparator();
            this.toolbox.AddTool(new LineTool());
            this.toolbox.AddTool(new RectangleTool());
            this.toolbox.AddTool(new FreeHandTool());

            if (plugins != null)
            {
                for (int i = 0; i < this.plugins.Length; i++)
                {
                    this.toolbox.Register(plugins[i]);
                }
            }

            this.toolbox.ToolSelected += Toolbox_ToolSelected;



            #endregion

            #region Toolbar

            // Initializing toolbar
            Debug.WriteLine("Loading toolbar...");
            this.toolbar = new DefaultToolbar();
            this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control)this.toolbar);
            
            ExampleToolbarItem toolItem1 = new ExampleToolbarItem();
            //toolItem1.SetCommand(whiteCanvasBgCmd);
            ExampleToolbarItem toolItem2 = new ExampleToolbarItem();
            //toolItem2.SetCommand(blackCanvasBgCmd);

            this.toolbar.AddToolbarItem(toolItem1);
            this.toolbar.AddSeparator();
            this.toolbar.AddToolbarItem(toolItem2);

            #endregion

        }

        private void Toolbox_ToolSelected(ITool tool)
        {
            if (this.editor != null)
            {
                Debug.WriteLine("Tool " + tool.Name + " is selected");
                ICanvas canvas = this.editor.GetSelectedCanvas();
                canvas.SetActiveTool(tool);
                tool.TargetCanvas = canvas;
            }
        }

    }
}
