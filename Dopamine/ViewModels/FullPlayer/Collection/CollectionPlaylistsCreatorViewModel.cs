﻿using Digimezzo.Utilities.Utils;
using Dopamine.Core.IO;
using Dopamine.Services.Entities;
using Dopamine.Services.Playlist;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dopamine.ViewModels.FullPlayer.Collection
{
    public class CollectionPlaylistsCreatorViewModel : BindableBase
    {
        private PlaylistType playlistType;
        private IPlaylistService playlistService;
        private ObservableCollection<SmartPlaylistLimit> limitTypes;
        private SmartPlaylistLimit selectedLimitType;

        private string playlistName;
        private ObservableCollection<SmartPlaylistRuleViewModel> rules;
        private int limit;
        private bool limitEnabled;
        private bool anyEnabled;

        public CollectionPlaylistsCreatorViewModel(IPlaylistService playlistService)
        {
            this.playlistService = playlistService;

            this.AddRuleCommand = new DelegateCommand(() => this.AddRule());
            this.RemoveRuleCommand = new DelegateCommand<SmartPlaylistRuleViewModel>((rule) => this.RemoveRule(rule));
            this.InitializeAsync();
        }

        public DelegateCommand AddRuleCommand { get; set; }

        public DelegateCommand<SmartPlaylistRuleViewModel> RemoveRuleCommand { get; set; }

        public ObservableCollection<SmartPlaylistRuleViewModel> Rules
        {
            get { return this.rules; }
            set { SetProperty<ObservableCollection<SmartPlaylistRuleViewModel>>(ref this.rules, value); }
        }

        public int Limit
        {
            get { return this.limit; }
            set { SetProperty<int>(ref this.limit, value); }
        }

        public bool LimitEnabled
        {
            get { return this.limitEnabled; }
            set { SetProperty<bool>(ref this.limitEnabled, value); }
        }

        public bool AnyEnabled
        {
            get { return this.anyEnabled; }
            set { SetProperty<bool>(ref this.anyEnabled, value); }
        }

        public PlaylistType PlaylistType
        {
            get { return this.playlistType; }
            set { SetProperty<PlaylistType>(ref this.playlistType, value); }
        }

        public string PlaylistName
        {
            get { return this.playlistName; }
            set { SetProperty<string>(ref this.playlistName, value); }
        }

        public ObservableCollection<SmartPlaylistLimit> LimitTypes
        {
            get { return this.limitTypes; }
            set { SetProperty<ObservableCollection<SmartPlaylistLimit>>(ref this.limitTypes, value); }
        }

        public SmartPlaylistLimit SelectedLimitType
        {
            get { return this.selectedLimitType; }
            set { SetProperty<SmartPlaylistLimit>(ref this.selectedLimitType, value); }
        }

        private async void InitializeAsync()
        {
            this.Rules = new ObservableCollection<SmartPlaylistRuleViewModel>();
            this.Rules.Add(new SmartPlaylistRuleViewModel());
            this.Limit = 25;
            this.PlaylistName = await this.playlistService.GetUniquePlaylistNameAsync(ResourceUtils.GetString("Language_New_Playlist"));

            this.limitTypes = new ObservableCollection<SmartPlaylistLimit>();
            this.limitTypes.Add(new SmartPlaylistLimit(SmartPlaylistLimitType.Songs, ResourceUtils.GetString("Language_Songs").ToLower()));
            this.limitTypes.Add(new SmartPlaylistLimit(SmartPlaylistLimitType.GigaBytes, ResourceUtils.GetString("Language_Gigabytes_Short")));
            this.limitTypes.Add(new SmartPlaylistLimit(SmartPlaylistLimitType.MegaBytes, ResourceUtils.GetString("Language_Megabytes_Short")));
            this.limitTypes.Add(new SmartPlaylistLimit(SmartPlaylistLimitType.Minutes, ResourceUtils.GetString("Language_Minutes").ToLower()));
            RaisePropertyChanged(nameof(this.LimitTypes));
            this.SelectedLimitType = this.LimitTypes.First();
        }

        private void AddRule()
        {
            this.Rules.Add(new SmartPlaylistRuleViewModel());
        }

        private void RemoveRule(SmartPlaylistRuleViewModel rule)
        {
            this.Rules.Remove(rule);
        }
    }
}
