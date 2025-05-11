using AutoMapper;
using Store.Es.Core;
using Store.Es.Core.Dtos.Products;
using Store.Es.Core.Entities;
using Store.Es.Core.Helper;
using Store.Es.Core.Services.Contract;
using Store.Es.Core.Specifications;
using Store.Es.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductSpecifications(specParams);
            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);
            var mappedProducts =  _mapper.Map<IEnumerable<ProductDto>>(products);
            var countSpec = new ProductWithCountSpecifications(specParams);
            int count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);
            return new PaginationResponse<ProductDto>(specParams.PageSize, specParams.PageIndex, count, mappedProducts);
        }
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecifications(id);
            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
           => _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());
        
        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
           => _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());
        

    }
}
