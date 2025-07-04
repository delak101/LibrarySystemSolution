﻿using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Services.Interfaces;

public interface IBookService
{
    Task<BookResponseDto?> GetBookByIdAsync(int bookId);
    Task<List<BookResponseDto>> GetBooksByNameAsync(string name);
    Task<List<BookResponseDto>> GetBooksAsync();
    Task<PagedResult<BookResponseDto>> GetBooksPagedAsync(int page, int pageSize);
    Task<PagedResult<BookResponseDto>> GetBooksByCategoryPagedAsync(string category, int page, int pageSize);
    Task<List<BookResponseDto>> GetBooksByGenreAsync(string genre);
    Task<List<BookResponseDto>> GetBooksByAuthorAsync(string author);
    Task<List<BookResponseDto>> GetBooksByAvailabilityAsync(bool isAvailable);
    Task<List<BookResponseDto>> GetBooksByYearAsync(int year);
    Task<List<BookResponseDto>> GetBooksByDepartmentAsync(string department);
    Task<List<Author>> GetAuthorsAsync();
    Task<List<Category>> GetCategoriesAsync();
    Task<BookResponseDto> AddBookAsync(BookDto bookDto);
    Task<BookResponseDto> UpdateBookAsync(int bookId, BookDto bookDto);
    Task<bool> DeleteBookAsync(int bookId);
}