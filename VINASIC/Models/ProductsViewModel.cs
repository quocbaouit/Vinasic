using System;

namespace VINASIC.ViewModels
{
    public class PpAndDecalModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public int Machining { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Quantity { get; set; }

        public float Lower5price { get; set; }
        public float Between5And20price { get; set; }

        public float Between20And50price { get; set; }

        public float Between50And100price { get; set; }

        public float Between100And200price { get; set; }

        public float Between200And500price { get; set; }
        public float Between500And1000price { get; set; }
        public float Over1000price { get; set; }
        public string ImagePath { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public string Description { get; set; }
        public PpAndDecalModel()
        {
            this.Id = Guid.NewGuid();
            this.Width = 0;
            this.Height = 0;
            this.Quantity = 1;
            //this.ImagePath = "https://congtystandee.com/image/cache/catalog/NAME%20CARD/CARD-VISIT-1000x1000.png";
        }

    }
    public class StampModel
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int Machining { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Square { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float SubTotal { get; set; }
        public float Total { get; set; }

        public StampModel()
        {
            this.Id = Guid.NewGuid();
            this.Machining = 0;
            this.Width = 0;
            this.Height = 0;
            this.Quantity = 1;
        }

    }
    public class StandeeModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }


        public float Quantity { get; set; }

        public float Lower5price { get; set; }
        public float Between5And10price { get; set; }

        public float Between10And20price { get; set; }

        public float Between20And50price { get; set; }


        public float Over50price { get; set; }
        public StandeeModel()
        {
            this.Id = Guid.NewGuid();
            this.Quantity = 1;
        }

    }
    public class DesignModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public float Lower2price { get; set; }
        public float Between2And5price { get; set; }
        public float Between5And10price { get; set; }
        public float Between10And20price { get; set; }
        public float Between20And30price { get; set; }
        public float Over30price { get; set; }
        public DesignModel()
        {
            this.Id = Guid.NewGuid();
            this.Quantity = 1;
        }

    }
    public class HiflexModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public int Machining { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Quantity { get; set; }

        public float Lower20price { get; set; }
        public float Between20And100price { get; set; }

        public float Between100And500price { get; set; }

        public float Between500And1000price { get; set; }

        public float Between1000And2000price { get; set; }
        
        public float Over2000price { get; set; }
        public HiflexModel()
        {
            this.Id = Guid.NewGuid();
            this.Width = 0;
            this.Height = 0;
            this.Quantity = 1;
        }

    }

    public class Paper
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public int Machining { get; set; }

        public float Quantity { get; set; }

        public float Between1And100price { get; set; }

        public float Between100And500price { get; set; }

        public float Between500And1000price { get; set; }


        public float Over50price { get; set; }
        public Paper()
        {
            this.Id = Guid.NewGuid();
            this.Machining = 0;
            this.Quantity = 1;
            
        }

    }


}
