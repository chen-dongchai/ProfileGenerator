using ProfileGenerator.Core.Models.Defination;

namespace ProfileGenerator.Core.Models.Arrangement
{
    public class StaggerArrange : NormalArrangeDefinition   //在网格排列的基础上增加偏移，所以内在逻辑仍是先完成网格排列，再偏移
    {
        public double StaggerOffset;
        public double StaggerOffsetFt;//偏移量
        //还需要偏移方向，按行偏移还是按列偏移☉ 按行偏移 (水平方向)   ○ 按列偏移 (垂直方向)
        public bool? IsRowOrColStagger = null;  //ture  按行偏移 (水平方向)   false 按列偏移 (垂直方向)
        //偏移起始，从奇数行/列  或者  偶数行/列 开始
        public bool? IsOddOrEvenStagger = null;//ture  从奇数行/列开始偏移   false 从偶数行/列开始偏移
        public StaggerArrange(double horizontalGap, double verticalGap, string unitname, int rows, int cols,
            double staggerOffset, bool? isRowOrColStagger, bool? isOddOrEvenStagger) : base(horizontalGap, verticalGap, unitname, rows, cols)
        {
            ArrangeTypeName = "交错";
            StaggerOffset = staggerOffset;
            IsRowOrColStagger = isRowOrColStagger;
            IsOddOrEvenStagger = isOddOrEvenStagger;
            this.Unitname = unitname;
            StaggerOffsetFt = Utils.Units.ToFeet(StaggerOffset, unitname);
        }

    }
}
