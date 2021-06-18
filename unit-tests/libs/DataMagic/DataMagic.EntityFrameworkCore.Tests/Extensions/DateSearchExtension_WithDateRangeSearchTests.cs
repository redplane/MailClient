﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models;
using DataMagic.Abstractions.Models.Filters;
using DataMagic.EntityFrameworkCore.Extensions;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Extensions
{
    // ReSharper disable once InconsistentNaming
    public class DateSearchExtension_WithDateRangeSearchTests
    {
        #region Static

        #region - Public

        public static IEnumerable<TestCaseData> UserBirthdayWithOperatorTestCaseData
        {
            get
            {
                // For Date to is null
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.Equal ), null, new List<User>
                    {
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) }
                    }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThan ), null, new List<User>
                    {
                        new() { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null }
                    }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThan ), null, new List<User>
                    {
                        new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) },
                        new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                    }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ), null, new List<User>
                    {
                        new() { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null },
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) }
                    }.AsQueryable( ) );
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ), null, new List<User>
                    {
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) },
                        new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) },
                        new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                    }.AsQueryable( ) );

                // For Date from is null.
                yield return new TestCaseData( null, new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThan ), new List<User>
                    {
                        new User { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null }
                    }.AsQueryable( ) );

                yield return new TestCaseData( null, new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThan ), new List<User>
                    {
                        new User { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) },
                        new User { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                    }.AsQueryable( ) );

                yield return new TestCaseData( null, new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.Equal ), new List<User>
                    {
                        new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                        new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) },
                    }.AsQueryable( ) );

                yield return new TestCaseData( null, new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ), new List<User>
                    {
                        new User { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null },
                        new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                        new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) }
                    }.AsQueryable( ) );

                yield return new TestCaseData( null, new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ), new List<User>
                    {
                        new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                        new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) },
                        new User { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) },
                        new User { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                    }.AsQueryable( ) );

                // For Date from operator is equal.
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.Equal ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.Equal ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.Equal ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new List<User>
                                                   {
                                                       new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                                                       new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.Equal ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.Equal ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.Equal ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new List<User>
                                                   {
                                                       new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                                                       new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) }
                                                   }.AsQueryable( ) );

                // For Date from operator is greater than.
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.Equal ),
                                               new List<User>
                                                   {
                                                       new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new List<User>
                                                   {
                                                       new User { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new List<User>
                                                   {
                                                       new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new List<User>
                                                   {
                                                       new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) },
                                                       new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );
              
                // For Date from operator is smaller than.
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.Equal ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new List<User>
                                                   {
                                                       new User { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new List<User>
                                                   {
                                                       new User { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null }
                                                   }.AsQueryable( ) );

                // For Date from operator is smaller than equal.
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.Equal ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new List<User>
                                                   {
                                                       new User { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null },
                                                       new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                                                       new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new List<User>
                                                   {
                                                       new User { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null },
                                                       new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                                                       new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) }
                                                   }.AsQueryable( ) );

                // // For Date from operator is greater than equal to.
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.Equal ),
                                               new List<User>
                                                   {
                                                       new User { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );
                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThan ),
                                               new List<User>( ).AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThan ),
                                               new List<User>
                                                   {
                                                       new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                                                       new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) },
                                                       new User { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new List<User>
                                                   {
                                                       new User { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new DateFilter( new Date( 2016, 1, 1 ), DateComparisonOperators.GreaterThanEqualTo ),
                                               new DateFilter( new Date( 2018, 1, 1 ), DateComparisonOperators.SmallerThanEqualTo ),
                                               new List<User>
                                                   {
                                                       new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                                                       new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) },
                                                       new User { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) },
                                                       new User { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                                                   }.AsQueryable( ) );
            }
        }

        #endregion

        #endregion

        #region Private

        private IQueryable<User> _users;

        #endregion

        #region Public

        [SetUp]
        public void Setup( )
        {
            var factory = new ConnectionFactory( );
            var context = factory.CreateContextForSQLite( );
            this._users = context.Users.AsQueryable( );
        }
        [Test]
        public void WithDateRangeSearch_PassNullFilter_ShouldReturnAllItems( )
        {
            // Arrange
            Expression<Func<User, DateTime>> x = user => user.Birthday;

            // Act
            var result = this._users.WithDateRangeSearch( x, null );

            // Assert
            result.Should( ).BeEquivalentTo( this._users );
        }

        [Test]
        public void WithDateRangeSearch_PassNullProperty_ShouldReturnAllItems( )
        {
            // Arrange
            var dateRangeFilter = new DateRangeFilter( It.IsAny<DateFilter>( ), It.IsAny<DateFilter>( ) );

            // Act
            var actualUsers = this._users.WithDateRangeSearch( null, dateRangeFilter );

            // Assert
            actualUsers.Should( ).BeEquivalentTo( this._users );
        }

        [TestCaseSource( nameof( UserBirthdayWithOperatorTestCaseData ) )]
        public void WithDateRangeSearch_PassRangedDate_ShouldReturnMatchedItems( DateFilter fromDate, DateFilter toDate, IQueryable<User> expectedUsers )
        {
            // Arrange
            var dateRangeFilter = new DateRangeFilter( fromDate, toDate );
            Expression<Func<User, DateTime>> x = user => user.Birthday;

            // Act
            var actualUsers = this._users.WithDateRangeSearch( x, dateRangeFilter );

            // Assert
            actualUsers.Should( ).BeEquivalentTo( expectedUsers );
        }

        #endregion
    }
}