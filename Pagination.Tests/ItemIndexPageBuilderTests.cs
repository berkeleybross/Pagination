﻿// <copyright file="ItemIndexPageBuilderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Pagination.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class ItemIndexPageBuilderTests
    {
        public class Constructor
            : ItemIndexPageBuilderTests
        {
            [Fact]
            public void Throws_exception_when_skip_is_less_than_0()
            {
                // Act
                Action act = () => new ItemIndexPageBuilder(-1, null);

                // Assert
                act.Should().Throw<ArgumentException>().WithMessage("skip must be greater than or equal to 0.\r\nParameter name: skip\r\nActual value was -1.");
            }

            [Fact]
            public void Throws_exception_when_take_is_less_than_1()
            {
                // Act
                Action act = () => new ItemIndexPageBuilder(0, 0);

                // Assert
                act.Should().Throw<ArgumentException>().WithMessage("take must be greater than or equal to 1.\r\nParameter name: take\r\nActual value was 0.");
            }

            [Fact]
            public void Throws_exception_when_skip_is_not_a_multiple_of_take()
            {
                // Act
                Action act = () => new ItemIndexPageBuilder(2, 5);

                // Assert
                act.Should().Throw<ArgumentException>().WithMessage("Skip must be a multiple of take");
            }
        }

        public class GetCurrentPage
            : ItemIndexPageBuilderTests
        {
            [Fact]
            public void Returns_empty_page_when_total_number_is_0_and_take_is_null()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, null);

                // Act
                var result = sut.GetCurrentPage(0);

                // Assert
                result.Should().BeEquivalentTo(new Page(1, int.MaxValue, true, -1, -1));
            }

            [Fact]
            public void Returns_empty_page_when_total_number_is_0_and_take_is_set()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, 5);

                // Act
                var result = sut.GetCurrentPage(0);

                // Assert
                result.Should().BeEquivalentTo(new Page(1, 5, true, -1, -1));
            }

            [Fact]
            public void When_take_is_null_sets_maximum_number_of_items_to_max_value()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, null);

                // Act
                var result = sut.GetCurrentPage(50);

                // Assert
                result.Should().BeEquivalentTo(new Page(1, int.MaxValue, true, 0, 49));
            }

            [Fact]
            public void When_take_is_null_and_skip_has_been_set_sets_maximum_number_of_items_to_max_value()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(10, null);

                // Act
                var result = sut.GetCurrentPage(50);

                // Assert
                result.Should().BeEquivalentTo(new Page(2, int.MaxValue, true, 10, 49));
            }

            [Fact]
            public void When_skip_is_zero_sets_page_number_to_1()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, 10);

                // Act
                var result = sut.GetCurrentPage(50);

                // Assert
                result.Should().BeEquivalentTo(new Page(1, 10, false, 0, 9));
            }

            [Theory]
            [InlineData(10, 2)]
            [InlineData(20, 3)]
            public void When_skip_is_greater_than_zero_sets_appropriate_page_number(int skip, int expected)
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(skip, 10);

                // Act
                var result = sut.GetCurrentPage(50);

                // Assert
                result.PageNumber.Should().Be(expected);
            }

            [Fact]
            public void When_skip_is_greater_than_total_count_still_skips_more()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(60, 10);

                // Act
                var result = sut.GetCurrentPage(50);

                // Assert
                result.Should().BeEquivalentTo(new Page(7, 10, false, -1, -1));
            }

            [Fact]
            public void When_skip_is_greater_than_total_count_resticts_to_last_page()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(60, 10, true);

                // Act
                var result = sut.GetCurrentPage(50);

                // Assert
                result.Should().BeEquivalentTo(new Page(5, 10, true, 40, 49));
            }

            [Fact]
            public void When_skip_is_greater_than_total_count_resticts_to_last_non_full_page()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(60, 10, true);

                // Act
                var result = sut.GetCurrentPage(53);

                // Assert
                result.Should().BeEquivalentTo(new Page(6, 10, true, 50, 52));
            }
        }

        public class GetPageContainingItem
            : ItemIndexPageBuilderTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            public void Throws_exception_when_total_number_of_items_is_less_than_or_equal_to_zero(int totalNumberOfItems)
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, null);

                // Act
                Action act = () => sut.GetPageContainingItem(0, totalNumberOfItems);

                // Assert
                act.Should().Throw<ArgumentException>()
                   .WithMessage($"totalNumberOfItems must be greater than or equal to 1.\r\nParameter name: totalNumberOfItems\r\nActual value was {totalNumberOfItems}.");
            }

            [Theory]
            [InlineData(26)]
            [InlineData(27)]
            public void Throws_exception_when_item_index_is_greater_than_the_total_number_of_items(int itemIndex)
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, null);

                // Act
                Action act = () => sut.GetPageContainingItem(itemIndex, 26);

                // Assert
                act.Should().Throw<ArgumentException>()
                   .WithMessage($"itemIndex must be less than 26.\r\nParameter name: itemIndex\r\nActual value was {itemIndex}.");
            }

            [Fact]
            public void When_take_is_null_sets_maximum_number_of_items_to_max_value()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(4, null);

                // Act
                var result = sut.GetPageContainingItem(0, 50);

                // Assert
                result.Should().BeEquivalentTo(new Page(1, int.MaxValue, true, 0, 49));
            }

            [Fact]
            public void When_take_is_null_and_skip_has_been_set_sets_maximum_number_of_items_to_max_value()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, null);

                // Act
                var result = sut.GetPageContainingItem(10, 50);

                // Assert
                result.Should().BeEquivalentTo(new Page(2, int.MaxValue, true, 10, 49));
            }

            [Fact]
            public void When_skip_is_zero_sets_page_number_to_1()
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(10, 10);

                // Act
                var result = sut.GetPageContainingItem(0, 50);

                // Assert
                result.Should().BeEquivalentTo(new Page(1, 10, false, 0, 9));
            }

            [Theory]
            [InlineData(10, 2)]
            [InlineData(11, 2)]
            [InlineData(15, 2)]
            [InlineData(19, 2)]
            [InlineData(20, 3)]
            public void When_item_index_is_greater_than_zero_sets_appropriate_page_number(int itemIndex, int expected)
            {
                // Arrange
                var sut = new ItemIndexPageBuilder(0, 10);

                // Act
                var result = sut.GetPageContainingItem(itemIndex, 50);

                // Assert
                result.PageNumber.Should().Be(expected);
            }
        }
    }
}