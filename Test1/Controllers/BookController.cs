using Ecommerce.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test1.Models;
using Test1.Request;

namespace Test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private TestContext _db;
        public BookController(TestContext db)
        {
            _db = db;
        }

        [HttpPost]
        public  async Task<IActionResult> CreateBook (BookRequest request )
        {
            if (request.BookName == "" || request.categoryNames.Count() == 0)
            {
                throw new CoreException("Đầu vào không hợp lệ");
            }
            // Bước 0 : Validate không cho add book trùng tên
            // kiểm tra trong db có thằng nào tên là conan (request.BookName)
            var bookDb = _db.Books.FirstOrDefault(x => x.BookName.Equals(request.BookName));

            if (bookDb is not null)
            {
                throw new CoreException("Book này đã tồn tại");
            }

            // Bước 1 : Thêm vào bảng Book
            var book = new Book();
            book.BookName = request.BookName;
            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            // Bước 2 : Thêm vào bảng Categori
            foreach(var item in request.categoryNames)
            {
                var category = new Category();
                category.CategoryName = item;
                category.BookId = book.Id;
                _db.Categories.Add(category);
            }
            await _db.SaveChangesAsync();
            return Ok();
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            // B1: Tìm xem trong db có quyển sách có Id đó không

            var book = await _db.Books.FirstOrDefaultAsync(x => x.Id == id);



            // Nếu không có thì báo lỗi 
            if (book is null)
            {
                throw new CoreException("Không tìm thấy quyển sách này trong db");
            }

            // Nếu có thì xóa
            // B1: Xóa category liên quan
            var categories = await _db.Categories.FirstOrDefaultAsync(x => x.BookId == id);
            if(categories is null)
            {
                throw new CoreException("Không tìm thấy thể loại trong db");
            }
            _db.Categories.RemoveRange(categories);
            // B2 : Xóa book
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(UpdateBook request)
        {
            // Xong bước lưu bOOK
            var book = await _db.Books.FirstOrDefaultAsync(x => x.Id == request.Book.Id);
            if(book is null)
            {
                throw new CoreException("Không tìm thấy quyển sách này trong db");
            }
            book.BookName = request.Book.BookName;
            _db.Books.Update(book);

            // Lưu categori
            // 1. Xóa hết categori cũ
            var categories = await _db.Categories.Where(x => x.BookId == book.Id).ToListAsync();
            if (book is null)
            {
                throw new CoreException("Không tìm thấy categori để xóa ");
            }
            _db.Categories.RemoveRange(categories);
            // 2. Thêm lại các category mới theo lựa chọn

            foreach (var item in request.Categories)
            {
                var category = new Category();
                category.CategoryName = item;
                category.BookId = book.Id;
                _db.Categories.Add(category);
            }
            await _db.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> SearchBook(string keyword)
        {
            var books = await _db.Books.Include(x => x.Categories).Where(x => x.BookName.Contains(keyword)).ToListAsync();
            return Ok(books);
        }






    }
}
