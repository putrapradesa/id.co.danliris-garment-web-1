﻿using Infrastructure.Domain;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts
{
    public class GarmentSubconDeliveryLetterOutItem : AggregateRoot<GarmentSubconDeliveryLetterOutItem, GarmentSubconDeliveryLetterOutItemReadModel>
    {
        

        public Guid SubconDeliveryLetterOutId { get; private set; }
        public int UENItemId { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }

        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public UomId UomOutId { get; private set; }
        public string UomOutUnit { get; private set; }

        public string FabricType { get; private set; }
        //
        public GarmentSubconDeliveryLetterOutItem(Guid identity, Guid subconDeliveryLetterOutId, int uENItemId, ProductId productId, string productCode, string productName, string productRemark, string designColor, double quantity, UomId uomId, string uomUnit, UomId uomOutId, string uomOutUnit, string fabricType) : base(identity)
        {
            Identity = identity;
            SubconDeliveryLetterOutId = subconDeliveryLetterOutId;
            UENItemId = uENItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            UomOutId = uomOutId;
            UomOutUnit = uomOutUnit;
            FabricType = fabricType;

            ReadModel = new GarmentSubconDeliveryLetterOutItemReadModel(Identity)
            {
                SubconDeliveryLetterOutId = SubconDeliveryLetterOutId,
                UENItemId = UENItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                ProductRemark = ProductRemark,
                FabricType = FabricType,

            };

            ReadModel.AddDomainEvent(new OnGarmentSubconDeliveryLetterOutPlaced(Identity));
        }

        public GarmentSubconDeliveryLetterOutItem(GarmentSubconDeliveryLetterOutItemReadModel readModel) : base(readModel)
        {
            UENItemId = readModel.UENItemId;
            SubconDeliveryLetterOutId = readModel.SubconDeliveryLetterOutId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ProductRemark = readModel.ProductRemark;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
            FabricType = readModel.FabricType;
            UomUnit = readModel.UomUnit;
            UomId = new UomId(readModel.UomId);
            UomOutId = new UomId(readModel.UomOutId);
            UomOutUnit = readModel.UomOutUnit;

        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconDeliveryLetterOutItem GetEntity()
        {
            return this;
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
    }
}
