using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace beRemote.GUI.Control.ConnectionTreeViewNS.ViewModel
{
  public static class ShopUtil
  {
    public static ObservableCollection<ShopCategory> CreateShop()
    {
      ObservableCollection<ShopCategory> list = new ObservableCollection<ShopCategory>();
      ShopCategory books = new ShopCategory("Books");
      ShopCategory movies = new ShopCategory("Movies");
      ShopCategory music = new ShopCategory("Music");

      //root categories
      list.Add(books);
      list.Add(movies);
      list.Add(music);

      //2nd level items on all categories
      books.SubCategories.Add(new ShopCategory("Fiction", books));
      books.SubCategories.Add(new ShopCategory("Travel", books));
      books.SubCategories.Add(new ShopCategory("Politics", books));

      movies.SubCategories.Add(new ShopCategory("Action", movies));
      movies.SubCategories.Add(new ShopCategory("Fantasy", movies));
      movies.SubCategories.Add(new ShopCategory("Romance", movies));
      movies.SubCategories.Add(new ShopCategory("Horror", movies));

      music.SubCategories.Add(new ShopCategory("Pop", music));
      music.SubCategories.Add(new ShopCategory("Techno", music));
      music.SubCategories.Add(new ShopCategory("Classical", music));
      music.SubCategories.Add(new ShopCategory("Ethno", music));
      ShopCategory rock = new ShopCategory("Rock", music);
      music.SubCategories.Add(rock);      
      
      //get 3rd level on rock
      rock.SubCategories.Add(new ShopCategory("Alternative", rock));
      rock.SubCategories.Add(new ShopCategory("Metal", rock));
      rock.SubCategories.Add(new ShopCategory("Industrial Rock", rock));

      

      return list;
    }
  }
}
