using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ChunkEditorXNA
{
    class TileMap
    {
        private Button[,] _buttonMap;
        private Dictionary<String, Texture2D> _texDict;
        private int _x, _y;

        public Button[,] ButtonMap
        {
            get { return _buttonMap; }
        }
        public int X
        {
            get { return _x; }
        }
        public int Y
        {
            get { return _y; }
        }

        public TileMap(int x, int y, int rows, int cols, int buttonWidth, int buttonHeight)
        {
            _x = x;
            _y = y;
            _buttonMap = new Button[rows, cols];
            _texDict = new Dictionary<string, Texture2D>();

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    _buttonMap[r, c] = new Button(_x + c * buttonWidth, _y + r * buttonHeight, buttonWidth, buttonHeight);

        }

        public void Update()
        {
            foreach (Button b in _buttonMap)
                b.Update();
        }
        public void Draw(SpriteBatch batch)
        {
            foreach (Button b in _buttonMap)
                b.Draw(batch);
        }
    }
}
