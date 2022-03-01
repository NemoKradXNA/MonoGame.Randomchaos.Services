using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Interfaces.Interfaces
{
    public interface IListItem
    {
        object Data { get; set; }
        string DisplayText { get; set; }
        string Format { get; set; }
        object DisplayValue { get; set; }

        Color DisplayColor { get; set; }
    }
}
