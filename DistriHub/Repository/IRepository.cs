using DistriHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistriHub.Repository
{
    public interface IRepository
    {
        Task<int> AddCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryByNameAsync(string name);
        Task<int> AddSubCategoryAsync(SubCategory subCategory);
        Task<SubCategory?> GetSubCategoryByNameAndCategoryAsync(string name, int categoryId);
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
        Task<int> AddModelAsync(Models.Model model);
        Task<Models.Model?> GetModelByNameAndSubCategoryAsync(string name, int subCategoryId);
        Task<Models.Model?> GetModelByNameAsync(string name);

        Task<Models.ProductDetails?> GetProductBySerialNoAsync(string serialNo);
        Task<IEnumerable<Models.ProductDetails>> GetProductDetailsAsync(string? serialFilter);
        Task<int> InsertProductDetailsAsync(Models.ProductDetails product);
        Task UpdateProductDetailsAsync(Models.ProductDetails product);
        Task UpdateProductDetailsDistributorsColsAsync(Models.ProductDetails product);
        Task UpdateProductDetailsDealerColsAsync(Models.ProductDetails product);
        Task<Models.Model?> GetModelByIdAsync(int id);

        // Serial number validation / unfreeze operations
        Task<int> ValidateSerialAsync(string materialCode, string serialNumber, string source);
        Task<int> UnfreezeSerialAsync(string materialCode, string serialNumber, string source);
        Task<string?> GetPasswordByUsernameAsync(string username);
        Task SetRefreshTokenAsync(string username, string refreshToken, DateTime expiry);
        Task<(string? RefreshToken, DateTime? Expiry)> GetRefreshTokenAsync(string username);
    }
}
