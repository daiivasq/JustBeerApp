﻿using JustBeerApp.Models;
using JustBeerApp.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace JustBeerApp.ViewModels
{
    public class BeerInfoPageViewModel : BaseViewModel, IInitialize
    {
        public string FavoriteIcon { get; set; }
        public string ID { get; set; }
        public Datum NewBeer { get; set; } = new Datum();
        public Data BeerInfo { get; set; }
        public DelegateCommand AddToFavoritesCommand { get; set; }
        public ObservableCollection<Beer> FavoriteList { get; set; }
        public BeerInfoPageViewModel(INavigationService navigation, IApiBeerService apiService, IPageDialogService pageDialogService) : base(navigation, apiService, pageDialogService)
        {
            GetBeerInfo();
            AddToFavoritesCommand = new DelegateCommand(async () =>
            {
                await AddToFavorites(BeerInfo.Beer);
            });
        }
        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Beer"))
            {
                NewBeer = parameters["Beer"] as Datum;
            }
        }

        //public override void Initialize(INavigationParameters parameters)
        //{
        //    try
        //    {
        //        if (!parameters.ContainsKey("DrinkId"))
        //        {
        //            ShowMessage(Constants.ErrorMessages.ErrorOccured, Constants.ErrorMessages.MissingParameter, Constants.ErrorMessages.Ok);
        //            return;
        //        }
        //        IsLoading = true;
        //        int drinkId = int.Parse(parameters["DrinkId"].ToString());
        //        if (parameters.ContainsKey("Cocktail"))
        //        {
        //            Cocktail = parameters["Cocktail"] as Cocktail;
        //        }
        //        else
        //        {
        //            GetCocktail(drinkId);
        //        }
        //        FavoriteIcon = Cocktail.IsFavorite ? FavoriteFilledIcon : FavoriteEmptyIcon;
        //        IsLoading = false;
        //    }
        //    catch (Exception e)
        //    {
        //        IsLoading = false;
        //        ShowMessage(Constants.ErrorMessages.ErrorOccured, e.Message, Constants.ErrorMessages.Ok);
        //    }
        //}
        public async Task GetBeerInfo()
        {
            bool internetAccess = await CheckInternetConnection();

            if (internetAccess)
            {
                try
                {
                    BeerInfo = await ApiService.GetBeers(ID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"API EXCEPTION {ex}");
                }
            }
        }

        public async Task AddToFavorites(Beer beerInformation)
        {
            FavoriteList.Add(beerInformation);
            FavoriteIcon = FavoriteList.Contains(beerInformation) ? "orangeHeart.png" : "favoritesIcon.png";
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var beer = parameters["Beer"] as Datum;
            ID = beer.Id;
        }
    }
}
