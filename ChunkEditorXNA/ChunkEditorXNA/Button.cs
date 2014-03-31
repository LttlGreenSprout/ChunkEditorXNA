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
    class Button : IComponent
    {
        #region variables
        private Rectangle _position;
        private String _text;
        private bool _prevPressed;
        private bool _currPressed;
        private bool _prevMouseDown;
        private Texture2D _texture;
        private SpriteFont _font;
        private Color _colour;

        public  event ClickHandler Click;
        public delegate void ClickHandler(object sender, MouseState ms);
        #endregion

        #region properties
        public Rectangle Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public int X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }
        public int Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }
        public int Width
        {
            get { return _position.Width; }
            set { _position.Width = value; }
        }
        public int Height
        {
            get { return _position.Height; }
            set { _position.Height = value; }
        }
        public bool Pressed
        {
            get { return _currPressed; }
        }
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }
        #endregion

        #region constructors
        public Button(Rectangle position)
        {
            _position = position;
            _text = "";
            _prevPressed = false;
            _currPressed = false;
            _colour = Color.White;
        }
        public Button(int x, int y, int width, int height)
        {
            _position = new Rectangle(x,y,width, height);
            _text = "";
            _prevPressed = false;
            _currPressed = false;
            _colour = Color.White;
        }
        #endregion

        #region IComponent methods
        /// <summary>
        /// Called once everyframe
        /// </summary>
        public void Update()
        {
            /**/
            MouseState mouseState = Mouse.GetState();
            if (_position.Contains(new Point(mouseState.X, mouseState.Y)))
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    _colour = Color.SlateGray;
                    _currPressed = true;
                }
                else
                {
                    _colour = Color.WhiteSmoke;
                    _currPressed = false;
                }
            else
            {
                _colour = Color.White;
                _currPressed = false;
            }
            if (_currPressed && !_prevPressed)
                this.Click(this, mouseState);
            _prevPressed = _currPressed;
            
            /**/
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, _position, _colour);
            if (_font != null)
                spriteBatch.DrawString(_font, _text, new Vector2(this.X + (this.Width - _font.MeasureString(_text).X)/2, this.Y+(this.Height - _font.MeasureString(_text).Y)/2), Color.Black);

        }
        #endregion
        

        
    }
}
