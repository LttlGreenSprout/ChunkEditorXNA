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
        public int Rows
        {
            get { return _buttonMap.GetLength(0); }
        }
        public int Cols
        {
            get { return _buttonMap.GetLength(1); }
        }
        public Button this[int rowIndex, int colIndex]
        {
            get { return _buttonMap[rowIndex, colIndex]; }
            set { _buttonMap[rowIndex, colIndex] = value; }
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
        public TileMap Clone()
        {
            TileMap cloneMap = new TileMap(_x, _y, _buttonMap.GetLength(0), _buttonMap.GetLength(1), _buttonMap[0,0].Width, _buttonMap[0,0].Height);
            for (int r = 0; r < _buttonMap.GetLength(0); r++)
                for (int c = 0; c < _buttonMap.GetLength(1); c++)
                    cloneMap[r, c] = _buttonMap[r, c].Clone();
            return cloneMap;
        }

        public override bool Equals(object obj)
        {
            TileMap b = obj as TileMap;
            if (b == null)
                return false;
            if (b.ButtonMap.GetLength(0) != this.ButtonMap.GetLength(0) || b.ButtonMap.GetLength(1) != this.ButtonMap.GetLength(1))
                return false;
            for (int r = 0; r < b.ButtonMap.GetLength(0); r++)
                for (int c = 0; c < b.ButtonMap.GetLength(1); c++)
                    if (b[r, c].Text != this[r, c].Text)
                        return false;
            return true;
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
