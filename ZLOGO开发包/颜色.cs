﻿using System.Drawing;
using ZLangRT.Attributes;

namespace ZLOGO开发包
{
    [ZMapping(typeof(Color))]
    public class 颜色
    {
        [ZCode("红色")]
        public Color Red { get; set; }

        [ZCode("白色")]
        public Color White { get; set; }

        [ZCode("黑色")]
        public Color Black { get; set; }

        [ZCode("黄色")]
        public Color Yellow { get; set; }

        [ZCode("蓝色")]
        public Color Blue { get; set; }

        [ZCode("绿色")]
        public Color Green { get; set; }

        [ZCode("褐色")]
        public Color Brown { get; set; } 
    }
}
