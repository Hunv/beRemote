using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace beRemote.GUI.Control.ConnectionTreeViewNS.ViewModel
{
  /// <summary>
  /// Provides a simplified view model for the sample
  /// application.
  /// </summary>
  public class ShopModel : INotifyPropertyChanged
  {
    private ObservableCollection<ShopCategory> categories;

    public ObservableCollection<ShopCategory> Categories
    {
      get { return categories; }
      set {
        categories = value;
        RaisePropertyChangedEvent("Categories");
      }
    }


    /// <summary>
    /// Refreshes the data.
    /// </summary>
    public void RefreshData()
    {
      Categories = ShopUtil.CreateShop();
    }


    ///<summary>
    ///Occurs when a property value changes.
    ///</summary>
    public event PropertyChangedEventHandler PropertyChanged;


    /// <summary>
    /// Fires the <see cref="PropertyChanged"/> event for a
    /// given property.
    /// </summary>
    /// <param name="propertyName">The changed property.</param>
    private void RaisePropertyChangedEvent(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }


    public ShopModel()
    {
      RefreshData();
    }

    public ShopCategory TryFindCategoryByName(string categoryName)
    {
      return TryFindCategoryByName(null, categoryName);
    }


    public ShopCategory TryFindCategoryByName(ShopCategory parent, string categoryName)
    {
      ObservableCollection<ShopCategory> cats;
      cats = parent == null ? categories : parent.SubCategories;
      foreach (ShopCategory category in cats)
      {
        if (category.CategoryName == categoryName)
        {
          return category;
        }
        else
        {
          ShopCategory cat = TryFindCategoryByName(category, categoryName);
          if (cat != null) return cat;
        }
      }

      return null;
    }
  }
}
