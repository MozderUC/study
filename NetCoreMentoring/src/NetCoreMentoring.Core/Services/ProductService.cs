﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetCoreMentoring.Core.Models;
using NetCoreMentoring.Core.Services.Contracts;
using NetCoreMentoring.Data;

namespace NetCoreMentoring.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;

        public ProductService(
            NorthwindContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<Products> GetProducts()
        {
            return _mapper.Map<IEnumerable<Products>>(_context.Products.Include(p => p.Category)
                .Include(p => p.Supplier).AsEnumerable());
        }
    }
}