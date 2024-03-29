﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morpher
{
    /*
     * Credit: Olivier Jacot-Descombes
     * https://stackoverflow.com/questions/1835062/drawing-circles-with-system-drawing
     */
    public static class GraphicsExtension
    {
        public static void DrawCircle(this Graphics g, Pen pen,
                                     float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }
    }
}
