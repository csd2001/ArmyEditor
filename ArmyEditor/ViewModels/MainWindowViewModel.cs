﻿using ArmyEditor.Logic;
using ArmyEditor.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ArmyEditor.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        IArmyLogic logic;
        public ObservableCollection<Trooper> Barrack { get; set; }
        public ObservableCollection<Trooper> Army { get; set; }
        private Trooper selectedFromBarracks;
        public Trooper SelectedFromBarracks
        {
            get { return selectedFromBarracks; }
            set 
            {
                SetProperty(ref selectedFromBarracks, value);
                (AddToArmyCommand as RelayCommand).NotifyCanExecuteChanged();
                (EditTrooperCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        private Trooper selectedFromArmy;
        public Trooper SelectedFromArmy
        {
            get { return selectedFromArmy; }
            set
            { 
                SetProperty(ref selectedFromArmy, value);
                (RemoveFromArmyCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public ICommand AddToArmyCommand { get; set; }
        public ICommand RemoveFromArmyCommand { get; set; }
        public ICommand EditTrooperCommand { get; set; }

        public int AllCost
        {
            get
            {
                return logic.AllCost;
            }
        }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public MainWindowViewModel()
            :this(IsInDesignMode ? null : Ioc.Default.GetService<IArmyLogic>())
        {

        }

        public MainWindowViewModel(IArmyLogic logic)
        {
            this.logic = logic;
            Barrack = new ObservableCollection<Trooper>();
            Army = new ObservableCollection<Trooper>();

            Barrack.Add(new Trooper()
            {
                Type = "marine",
                Power = 8,
                Vitality = 6,
                Cost = 4
            });
            Barrack.Add(new Trooper()
            {
                Type = "pilot",
                Power = 7,
                Vitality = 3,
                Cost = 10
            });
            Barrack.Add(new Trooper()
            {
                Type = "infantry",
                Power = 6,
                Vitality = 8,
                Cost = 10
            });
            Barrack.Add(new Trooper()
            {
                Type = "sniper",
                Power = 3,
                Vitality = 3,
                Cost = 1
            });
            Barrack.Add(new Trooper()
            {
                Type = "engineer",
                Power = 5,
                Vitality = 6,
                Cost = 3
            });

            Army.Add(Barrack[1].GetCopy());
            Army.Add(Barrack[2].GetCopy());

            logic.SetupCollections(Barrack, Army);

            AddToArmyCommand = new RelayCommand(
                () => logic.AddToArmy(SelectedFromBarracks),
                () => SelectedFromBarracks != null
                ) ;

            RemoveFromArmyCommand = new RelayCommand(
                () => logic.RemoveFromArmy(SelectedFromArmy),
                () => SelectedFromArmy != null
                );

            EditTrooperCommand = new RelayCommand(
                () => logic.EditTrooper(SelectedFromBarracks),
                () => SelectedFromBarracks != null
                );

            Messenger.Register<MainWindowViewModel, string, string>(this, "TrooperInfo", (recipient, msg) =>
             {
                 OnPropertyChanged("AllCost");
             });
        }
    }
}
