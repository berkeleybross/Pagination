﻿// <copyright file="PaginationHelpersTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Pagination.Tests
{
    using System.Collections.Immutable;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PaginationHelpersTests
    {
        private class PageEnumerable
            : PaginationHelpersTests
        {
            [Test]
            public void Returns_empty_pagedlist_when_given_empty_source()
            {
                // Arrange
                var source = Enumerable.Empty<int>();

                // Act
                var result = source.Page(2, null);

                // Assert
                var currentPage = new Page(2, int.MaxValue, true, -1, -1);
                result.ShouldBeEquivalentTo(PagedList<int>.Empty(0, currentPage));
            }

            [Test]
            public void Skips_appropriate_number()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                // Act
                var result = source.Page(2, null);

                // Assert
                var currentPage = new Page(2, int.MaxValue, true, 2, 8);
                result.ShouldBeEquivalentTo(PagedList<int>.Create(9, currentPage, new[] { 3, 4, 5, 6, 7, 8, 9 }));
            }

            [Test]
            public void Takes_appropriate_number()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                // Act
                var result = source.Page(0, 2);

                // Assert
                var currentPage = new Page(1, 2, false, 0, 1);
                result.ShouldBeEquivalentTo(PagedList<int>.Create(9, currentPage, new[] { 1, 2 }));
            }

            [Test]
            public void Skips_and_takes_appropriate_number()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                // Act
                var result = source.Page(4, 2);

                // Assert
                result.ShouldBeEquivalentTo(PagedList<int>.Create(9, new Page(3, 2, false, 4, 5), new[] { 5, 6 }));
            }

            [Test]
            public void Allows_page_to_be_past_last_item()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                // Act
                var result = source.Page(50, 2);

                // Assert
                result.ShouldBeEquivalentTo(PagedList<int>.Empty(9, new Page(26, 2, false, -1, -1)));
            }
        }

        private class PageQueryable
            : PaginationHelpersTests
        {
            [Test]
            public void Returns_empty_pagedlist_when_given_empty_source()
            {
                // Arrange
                var source = Enumerable.Empty<int>().AsQueryable();

                // Act
                var result = source.Page(2, null);

                // Assert
                var currentPage = new Page(2, int.MaxValue, true, -1, -1);
                result.ShouldBeEquivalentTo(PagedList<int>.Empty(0, currentPage));
            }

            [Test]
            public void Skips_appropriate_number()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();

                // Act
                var result = source.Page(2, null);

                // Assert
                var currentPage = new Page(2, int.MaxValue, true, 2, 8);
                result.ShouldBeEquivalentTo(PagedList<int>.Create(9, currentPage, new[] { 3, 4, 5, 6, 7, 8, 9 }));
            }

            [Test]
            public void Takes_appropriate_number()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();

                // Act
                var result = source.Page(0, 2);

                // Assert
                var currentPage = new Page(1, 2, false, 0, 1);
                result.ShouldBeEquivalentTo(PagedList<int>.Create(9, currentPage, new[] { 1, 2 }));
            }

            [Test]
            public void Skips_and_takes_appropriate_number()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();

                // Act
                var result = source.Page(4, 2);

                // Assert
                result.ShouldBeEquivalentTo(PagedList<int>.Create(9, new Page(3, 2, false, 4, 5), new[] { 5, 6 }));
            }

            [Test]
            public void Allows_page_to_be_past_last_item()
            {
                // Arrange
                var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();

                // Act
                var result = source.Page(50, 2);

                // Assert
                result.ShouldBeEquivalentTo(PagedList<int>.Empty(9, new Page(26, 2, false, -1, -1)));
            }
        }
    }
}