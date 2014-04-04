using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
namespace ChunkEditorXNA
{
    public class ChunkEditorManager
    {
        #region constants
        private const int BUTTON_FILE_WIDTH = 50;
        private const int BUTTON_FILE_HEIGHT = 20;
        private const int BUTTON_TILE_WIDTH = 50;
        private const int BUTTON_TILE_HEIGHT = 50;
        #endregion
        #region readonlys
        private readonly Dictionary<Keys, String> KEY_STRING_LOWERCASE;
        private readonly Dictionary<Keys, String> KEY_STRING_UPPERCASE;
        #endregion

        ContentManager content;
        SpriteBatch spriteBatch;
        TileMap tileMap;
        Texture2D buttonTexture;
        SpriteFont font;
        String currentString;
        Button btnSave, btnOpen, btnUndo, btnRedo;
        Stack<TileMap> undoStack, redoStack;
        MouseState prevMouseState;
        bool updateUndoStack = false;

        //debug vars
        List<IComponent> components;
        Button testButton;

        #region properties
        private ContentManager Content
        {
            get { return content; }
        }
        #endregion
        public ChunkEditorManager(ContentManager gameManager)
        {
            content = gameManager;
            components = new List<IComponent>();
            undoStack = new Stack<TileMap>();
            redoStack = new Stack<TileMap>();
            tileMap = new TileMap(0, 50, 4, 8, BUTTON_TILE_WIDTH, BUTTON_TILE_HEIGHT);
            //tileMap = new TileMap(50, 0, 4, 8, BUTTON_TILE_WIDTH, BUTTON_TILE_HEIGHT);
            btnSave = new Button(0, 0, BUTTON_FILE_WIDTH, BUTTON_FILE_HEIGHT);
            btnOpen = new Button(BUTTON_FILE_WIDTH, 0, BUTTON_FILE_WIDTH, BUTTON_FILE_HEIGHT);
            btnUndo = new Button(2 * BUTTON_FILE_WIDTH, 0, BUTTON_FILE_WIDTH, BUTTON_FILE_HEIGHT);
            btnRedo = new Button(3 * BUTTON_FILE_WIDTH, 0, BUTTON_FILE_WIDTH, BUTTON_FILE_HEIGHT);
            currentString = "#";

            KEY_STRING_LOWERCASE = new Dictionary<Keys, string>();
            #region assign regular alphabet
            KEY_STRING_LOWERCASE[Keys.A] = "a";
            KEY_STRING_LOWERCASE[Keys.B] = "b";
            KEY_STRING_LOWERCASE[Keys.C] = "c";
            KEY_STRING_LOWERCASE[Keys.D] = "d";
            KEY_STRING_LOWERCASE[Keys.E] = "e";
            KEY_STRING_LOWERCASE[Keys.F] = "f";
            KEY_STRING_LOWERCASE[Keys.G] = "g";
            KEY_STRING_LOWERCASE[Keys.H] = "h";
            KEY_STRING_LOWERCASE[Keys.I] = "i";
            KEY_STRING_LOWERCASE[Keys.J] = "j";
            KEY_STRING_LOWERCASE[Keys.K] = "k";
            KEY_STRING_LOWERCASE[Keys.L] = "l";
            KEY_STRING_LOWERCASE[Keys.M] = "m";
            KEY_STRING_LOWERCASE[Keys.N] = "n";
            KEY_STRING_LOWERCASE[Keys.O] = "o";
            KEY_STRING_LOWERCASE[Keys.P] = "p";
            KEY_STRING_LOWERCASE[Keys.Q] = "q";
            KEY_STRING_LOWERCASE[Keys.R] = "r";
            KEY_STRING_LOWERCASE[Keys.S] = "s";
            KEY_STRING_LOWERCASE[Keys.T] = "t";
            KEY_STRING_LOWERCASE[Keys.U] = "u";
            KEY_STRING_LOWERCASE[Keys.V] = "v";
            KEY_STRING_LOWERCASE[Keys.W] = "w";
            KEY_STRING_LOWERCASE[Keys.X] = "x";
            KEY_STRING_LOWERCASE[Keys.Y] = "y";
            KEY_STRING_LOWERCASE[Keys.Z] = "z";
            #endregion
            #region assign number keys (no numpad)
            KEY_STRING_LOWERCASE[Keys.D1] = "1";
            KEY_STRING_LOWERCASE[Keys.D2] = "2";
            KEY_STRING_LOWERCASE[Keys.D3] = "3";
            KEY_STRING_LOWERCASE[Keys.D4] = "4";
            KEY_STRING_LOWERCASE[Keys.D5] = "5";
            KEY_STRING_LOWERCASE[Keys.D6] = "6";
            KEY_STRING_LOWERCASE[Keys.D7] = "7";
            KEY_STRING_LOWERCASE[Keys.D8] = "8";
            KEY_STRING_LOWERCASE[Keys.D9] = "9";
            KEY_STRING_LOWERCASE[Keys.D0] = "0";
            #endregion
            #region assign special characters
            KEY_STRING_LOWERCASE[Keys.OemTilde] = "`";
            KEY_STRING_LOWERCASE[Keys.OemMinus] = "-";
            KEY_STRING_LOWERCASE[Keys.OemPlus] = "=";

            KEY_STRING_LOWERCASE[Keys.OemOpenBrackets] = "[";
            KEY_STRING_LOWERCASE[Keys.OemCloseBrackets] = "]";
            KEY_STRING_LOWERCASE[Keys.OemPipe] = "\\";

            KEY_STRING_LOWERCASE[Keys.OemSemicolon] = ";";
            KEY_STRING_LOWERCASE[Keys.OemQuotes] = "'";

            KEY_STRING_LOWERCASE[Keys.OemComma] = ",";
            KEY_STRING_LOWERCASE[Keys.OemPeriod] = ".";
            KEY_STRING_LOWERCASE[Keys.OemQuestion] = "/";
            #endregion
            KEY_STRING_UPPERCASE = new Dictionary<Keys, string>();
            #region assign regular alphabet
            KEY_STRING_UPPERCASE[Keys.A] = "A";
            KEY_STRING_UPPERCASE[Keys.B] = "B";
            KEY_STRING_UPPERCASE[Keys.C] = "C";
            KEY_STRING_UPPERCASE[Keys.D] = "D";
            KEY_STRING_UPPERCASE[Keys.E] = "E";
            KEY_STRING_UPPERCASE[Keys.F] = "F";
            KEY_STRING_UPPERCASE[Keys.G] = "G";
            KEY_STRING_UPPERCASE[Keys.H] = "H";
            KEY_STRING_UPPERCASE[Keys.I] = "I";
            KEY_STRING_UPPERCASE[Keys.J] = "J";
            KEY_STRING_UPPERCASE[Keys.K] = "K";
            KEY_STRING_UPPERCASE[Keys.L] = "L";
            KEY_STRING_UPPERCASE[Keys.M] = "M";
            KEY_STRING_UPPERCASE[Keys.N] = "N";
            KEY_STRING_UPPERCASE[Keys.O] = "O";
            KEY_STRING_UPPERCASE[Keys.P] = "P";
            KEY_STRING_UPPERCASE[Keys.Q] = "Q";
            KEY_STRING_UPPERCASE[Keys.R] = "R";
            KEY_STRING_UPPERCASE[Keys.S] = "S";
            KEY_STRING_UPPERCASE[Keys.T] = "T";
            KEY_STRING_UPPERCASE[Keys.U] = "U";
            KEY_STRING_UPPERCASE[Keys.V] = "V";
            KEY_STRING_UPPERCASE[Keys.W] = "W";
            KEY_STRING_UPPERCASE[Keys.X] = "X";
            KEY_STRING_UPPERCASE[Keys.Y] = "Y";
            KEY_STRING_UPPERCASE[Keys.Z] = "Z";
            #endregion
            #region assign number keys (no numpad)
            KEY_STRING_UPPERCASE[Keys.D1] = "!";
            KEY_STRING_UPPERCASE[Keys.D2] = "@";
            KEY_STRING_UPPERCASE[Keys.D3] = "#";
            KEY_STRING_UPPERCASE[Keys.D4] = "$";
            KEY_STRING_UPPERCASE[Keys.D5] = "%";
            KEY_STRING_UPPERCASE[Keys.D6] = "^";
            KEY_STRING_UPPERCASE[Keys.D7] = "&";
            KEY_STRING_UPPERCASE[Keys.D8] = "*";
            KEY_STRING_UPPERCASE[Keys.D9] = "(";
            KEY_STRING_UPPERCASE[Keys.D0] = ")";
            #endregion
            #region assign special characters
            KEY_STRING_UPPERCASE[Keys.OemTilde] = "~";
            KEY_STRING_UPPERCASE[Keys.OemMinus] = "_";
            KEY_STRING_UPPERCASE[Keys.OemPlus] = "+";

            KEY_STRING_UPPERCASE[Keys.OemOpenBrackets] = "{";
            KEY_STRING_UPPERCASE[Keys.OemCloseBrackets] = "}";
            KEY_STRING_UPPERCASE[Keys.OemPipe] = "|";

            KEY_STRING_UPPERCASE[Keys.OemSemicolon] = ":";
            KEY_STRING_UPPERCASE[Keys.OemQuotes] = "\"";

            KEY_STRING_UPPERCASE[Keys.OemComma] = "<";
            KEY_STRING_UPPERCASE[Keys.OemPeriod] = ">";
            KEY_STRING_UPPERCASE[Keys.OemQuestion] = "?";
            #endregion

            //add other components to be updated and drawn
            components.Add(btnSave);
            components.Add(btnOpen);
            components.Add(btnUndo);
            components.Add(btnRedo);

            prevMouseState = Mouse.GetState();
            

            //TileMap fofuckyourself = new TileMap(50,50,1,1,20,20);
            //ConfigureTileMap(fofuckyourself);
            //fofuckyourself[0, 0].Text = "%";
            //undoStack.Push(fofuckyourself);
        }
        public void LoadContent(SpriteBatch batch)
        {
            spriteBatch = batch;
            buttonTexture = this.Content.Load<Texture2D>("defaultWhite");
            font = this.Content.Load<SpriteFont>("SpriteFont1");
            //testButton.Texture = buttonTexture;
            //testButton.Font = font;
            ConfigureTileMap();
            //save button
            btnSave.Texture = buttonTexture;
            btnSave.Font = font;
            btnSave.Text = "Save";
            btnSave.Click += SaveButton_Click;
            //open button
            btnOpen.Texture = buttonTexture;
            btnOpen.Font = font;
            btnOpen.Text = "Open";
            btnOpen.Click += OpenButton_Click;
            //undo button
            btnUndo.Texture = buttonTexture;
            btnUndo.Font = font;
            btnUndo.Text = "Undo";
            btnUndo.Click += UndoButton_Click;
            //redo button
            btnRedo.Texture = buttonTexture;
            btnRedo.Font = font;
            btnRedo.Text = "Redo";
            btnRedo.Click += RedoButton_Click;

            PushTileMapOntoStack(tileMap, undoStack);
        }
        private void ConfigureTileMap()
        {
            ConfigureTileMap(tileMap);
        }
        private void ConfigureTileMap(TileMap map)
        {
            foreach (Button b in map.ButtonMap)
            {
                b.Texture = buttonTexture;
                b.Font = font;
                b.Text = "#";
                b.Click += TileButton_Click;
            }
            //undoStack.Push(map.Clone());
        }
        public void Update()
        {
            ProcessKeyboardInput();
            tileMap.Update();
            
            //update components
            foreach (IComponent c in components)
                c.Update();
            MouseState currMouseState = Mouse.GetState();
            if (prevMouseState.LeftButton == ButtonState.Pressed && currMouseState.LeftButton == ButtonState.Released)
                ProcessMouseRelease();
            prevMouseState = currMouseState;
        }
        public void Draw()
        {
            tileMap.Draw(spriteBatch);
            //undoStack.Peek().Draw(spriteBatch);
            
            spriteBatch.DrawString(font, currentString, new Vector2(0,tileMap.Y+tileMap.ButtonMap.GetLength(0)*BUTTON_TILE_HEIGHT), Color.Black);
            spriteBatch.DrawString(font, tileMap.ButtonMap[0, 0].Text, new Vector2(50, 0), Color.Black);

            foreach (IComponent c in components)
                c.Draw(spriteBatch);

            spriteBatch.DrawString(font, tileMap.Equals(tileMap).ToString(), new Vector2(200, 0), Color.Black);
            spriteBatch.DrawString(font, undoStack.Count.ToString(), new Vector2(250, 0), Color.Black);
            spriteBatch.DrawString(font, redoStack.Count.ToString(), new Vector2(300, 0), Color.Black);
        }
        //Handles Mouse Releases for undo Stack
        private void ProcessMouseRelease()
        {
            //if(undoStack.Count > 0 && tileMap.Equals(undoStack.Peek()) == true)
            //    return;
            if (updateUndoStack)
            {
                PushTileMapOntoStack(tileMap, undoStack);
                /*
                TileMap temp = new TileMap(tileMap.X, tileMap.Y, tileMap.Rows, tileMap.Cols, BUTTON_TILE_WIDTH, BUTTON_TILE_HEIGHT);
                for (int r = 0; r < temp.Rows; r++)
                    for (int c = 0; c < temp.Cols; c++)
                        temp[r, c].Text = tileMap[r, c].Text;
                undoStack.Push(temp);
                /**/
                updateUndoStack = false;
            }
            
        }
        private void PushTileMapOntoStack(TileMap map, Stack<TileMap> stack)
        {
            TileMap temp = map.Clone();
            ConfigureTileMap(temp);
            for (int r = 0; r < temp.Rows; r++)
                for (int c = 0; c < temp.Cols; c++)
                    temp[r, c].Text = map[r, c].Text;
            stack.Push(temp);
        }
        //Handles all keyboard input
        private void ProcessKeyboardInput()
        {
            KeyboardState kbs = Keyboard.GetState();
            Keys[] keys = kbs.GetPressedKeys();

            //shift keys
            if (kbs.IsKeyDown(Keys.LeftShift) || kbs.IsKeyDown(Keys.RightShift) || Console.CapsLock)
                foreach (Keys k in keys)
                {
                    try { currentString = KEY_STRING_UPPERCASE[k]; }
                    catch { }
                }
            // non-shift keys
            else
                foreach (Keys k in keys)
                {
                        try { currentString = KEY_STRING_LOWERCASE[k]; }
                        catch { }
                }
        }
        #region Button Click Methods
        public void SaveButton_Click(object sender, MouseState ms)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = ".txt";
            DialogResult result = saveFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.OK:
                    SaveChunkToText(saveFileDialog.FileName);
                    break;
            }
        }
        public void OpenButton_Click(object sender, MouseState ms)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.DefaultExt = ".txt";
            DialogResult result = openFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.OK:
                    OpenChunkFromText(openFileDialog.FileName);
                    break;
            }
        }
        public void UndoButton_Click(object sender, MouseState ms)
        {
            if (undoStack.Count > 1)
            {
                PushTileMapOntoStack(tileMap, redoStack);
                //tileMap = undoStack.Pop().Clone();
                TileMap temp = undoStack.Pop();
                for (int r = 0; r < temp.ButtonMap.GetLength(0); r++)
                    for (int c = 0; c < temp.ButtonMap.GetLength(1); c++)
                        tileMap[r, c] = temp[r, c];
                tileMap = temp;
            }
            
        }
        public void RedoButton_Click(object sender, MouseState ms)
        {
            if (redoStack.Count > 0)
            {
                PushTileMapOntoStack(tileMap, undoStack);
                tileMap = redoStack.Pop();
            }
        }
        public void TileButton_Click(object sender, MouseState ms)
        {
            Button b = sender as Button;
            if (b == null)
                return;
            b.Text = currentString;
            updateUndoStack = true;
            
        }
        #endregion
        #region IO
        public void SaveChunkToText(String filename)
        {
            try
            {
                using(StreamWriter writer = new StreamWriter(filename))
                {
                    for(int r = 0; r < tileMap.ButtonMap.GetLength(0); r++)
                    {
                        for(int c = 0; c < tileMap.ButtonMap.GetLength(1); c++)
                        {
                            writer.Write(tileMap.ButtonMap[r, c].Text);
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch
            {
                MessageBox.Show("SOMETHING \"BadImageFormatException\" HAPPENED while saving");
            }
        }
        public void OpenChunkFromText(String filename)
        {
            try
            {
                tileMap = new TileMap(tileMap.X, tileMap.Y, tileMap.ButtonMap.GetLength(0), tileMap.ButtonMap.GetLength(1), BUTTON_TILE_WIDTH, BUTTON_TILE_HEIGHT);
                this.ConfigureTileMap();
                using (StreamReader reader = new StreamReader(filename))
                {
                    for (int r = 0; r < tileMap.ButtonMap.GetLength(0); r++)
                    {
                        for (int c = 0; c < tileMap.ButtonMap.GetLength(1); c++)
                        {
                            tileMap.ButtonMap[r, c].Text = ((char)reader.Read()).ToString();
                        }
                        reader.ReadLine();
                    }
                }
                
            }
            catch
            {
                MessageBox.Show("SOMETHING \"bad\" HAPPENED while opening");
            }
        }
        #endregion
    }
}
