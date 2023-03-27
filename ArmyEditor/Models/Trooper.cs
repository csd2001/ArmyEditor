using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyEditor.Models
{
    public class Trooper : ObservableObject
    {
        private string type;
        public string Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        private int power;
        public int Power
        {
            get { return power; }
            set { SetProperty(ref power, value); }
        }

        private int vitality;
        public int Vitality
        {
            get { return vitality; }
            set { SetProperty(ref vitality, value); }
        }
        private int cost;
        public int Cost
        {
            get { return cost; }
            set { SetProperty(ref cost, value); }
        }



        public Trooper GetCopy()
        {
            return new Trooper()
            {
                Type = this.Type,
                Power = this.Power,
                Vitality = this.Vitality,
                Cost = this.Cost
            };
        }


    }
}
