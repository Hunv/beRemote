using System;
using System.Collections.ObjectModel;

namespace beRemote.GUI.Control.ConnectionTreeViewNS.ViewModel
{
  /// <summary>
  /// Represents an article category.
  /// </summary>
  public class ShopCategory
  {
    private readonly ObservableCollection<ShopCategory> subCategories = new ObservableCollection<ShopCategory>();

    private string categoryName = String.Empty;

    private ShopCategory parentCategory;


    /// <summary>
    /// The nested categories.
    /// </summary>
    public ObservableCollection<ShopCategory> SubCategories
    {
      get { return subCategories; }
    }


    /// <summary>
    /// The name of the category. This sample also used
    /// the name as the category key.
    /// </summary>
    public string CategoryName
    {
      get { return categoryName; }
      set { categoryName = value; }
    }


    /// <summary>
    /// The parent category, if any. If this is
    /// a top-level category, this property returns null.
    /// </summary>
    public ShopCategory ParentCategory
    {
      get { return parentCategory; }
      set { parentCategory = value; }
    }


    /// <summary>
    /// Creates a category without a parent.
    /// </summary>
    /// <param name="categoryName">The category's name.</param>
    public ShopCategory(string categoryName) : this(categoryName, null)
    {
    }



    /// <summary>
    /// Creates a category for a given parent. This sets the
    /// <see cref="ParentCategory"/> reference, but does not
    /// automatically add the category to the parent's
    /// <see cref="SubCategories"/> collection.
    /// </summary>
    /// <param name="categoryName">The category's name.</param>
    /// <param name="parentCategory">The parent category, if any.</param>
    public ShopCategory(string categoryName, ShopCategory parentCategory)
    {
      this.categoryName = categoryName;
      this.parentCategory = parentCategory;
    }


    /// <summary>
    /// Returns a string with the <see cref="CategoryName"/>.
    /// </summary>
    /// <returns>The category name.</returns>
    public override string ToString()
    {
      return "Category: " + categoryName;
    }
  }
}