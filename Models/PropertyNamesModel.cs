using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisTools.Models
{
    internal class PropertyNamesModel
    {
        public string[] VolumeNames { get; set; }
        public string[] AreaNames { get; set; }
        public string[] LengthNames { get; set; }
        public string[] CategoryNames { get; set; }

        // Singleton instance
        private static PropertyNamesModel _instance;

        public static PropertyNamesModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PropertyNamesModel
                    {
                        VolumeNames = new string[]
                        {
                            "Объем",
                            "Volume",
                            "Объем (м3)",
                            "Volume (m³)"
                        },
                        AreaNames = new string[]
                        {
                            "Площадь",
                            "Area",
                            "Площадь (м2)",
                            "Area (m²)"
                        },
                        LengthNames = new string[]
                        {
                            "Длина",
                            "Length",
                            "Длина (м)",
                            "Length (m)"
                        },
                        CategoryNames = new string[]
                        {
                            "Element",
                            "Объект",
                            "Материал",
                            "Revit Material",
                            "Autodesk Material",
                            "Dimensions",
                            "Identity Data"
                        }
                    };
                }
                return _instance;
            }
        }
    }
}
