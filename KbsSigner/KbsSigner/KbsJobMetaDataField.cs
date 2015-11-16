using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbsSigner
{
    public enum EnumJobMetaField
    {
        None = 0,

        ProjectNameAndAddress,

        Lot,

        Block,

        County,

        ContractorBuilder,

        Model,

        Elevation,

        Options,

        OccupancySingleFamily,

        OccupancyMultiFamily,

        OccupancyCommercial,

        DesignCriteria,

        RfTopChordLiveLoad,

        RfTopChordDeadLoad,

        RfBottomChordLiveLoad,

        RfBottomChordDeadLoad,

        RfDurationFactor,

        MeanHeight,

        Exposure,

        FlrTopChordLiveLoad,

        FlrTopChordDeadLoad,

        FlrBottomChordLiveLoad,

        FlrBottomChordDeadLoad,

        FlrDurationFactor
    }

    public class KbsJobMetaDataField
    {
        public EnumJobMetaField FieldType { get; set; }

        public string FieldValue { get; set; }

        public KbsPoint FieldPosition { get; set; }

        public static KbsJobMetaDataField Create(EnumJobMetaField fieldType, string fieldValue)
        {
            KbsJobMetaDataField field = new KbsJobMetaDataField
            {
                FieldValue = fieldValue, 
                FieldType = fieldType
            };

            return field;
        }
    }
}
