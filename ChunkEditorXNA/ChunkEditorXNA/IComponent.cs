using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ChunkEditorXNA
{
    interface IComponent
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}
