
using ZLangRT.Attributes;
using ZLogoEngine;

namespace ZLOGO开发包
{
    [ZMapping(typeof(TurtleForm))]
    public abstract class 绘图窗体
    {
        [ZCode("小海龟")]
        public TurtleSprite Turtle { get; set; }

        [ZCode("窗口")]
        public TurtleForm Window { get; set; }

        [ZCode("开始绘图")]
        public abstract void RunZLogo();
        
        //[ZCode("标题")]
        //public Form Text { get; set; }
    
        [ZCode("设置标题为(string:title)")]
        public abstract void SetTitle(string title);

    }
}
