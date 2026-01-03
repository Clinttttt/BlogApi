using AutoMapper;
using BlogApi.Application.Commands.Tags.AddTag;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BlogApi.UnitTest.Commands.Tags
{
    public class AddTagCommandHandlerIntegrationTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly Mock<IMapper> _mockMapper;

        public AddTagCommandHandlerIntegrationTests()
        {
          
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<bool>(It.IsAny<Tag>())).Returns(true);
        }

        [Fact]
        public async Task Handle_NewTag_ShouldAddToDatabase()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var handler = new AddTagCommandHandler(context);
            var command = new AddTagCommand("Technology", Guid.NewGuid());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, await context.Tags.CountAsync());

            var addedTag = await context.Tags.FirstAsync();
            Assert.Equal("Technology", addedTag.Name);
        }

        [Fact]
        public async Task Handle_DuplicateTag_ShouldReturnConflict()
        {
            // Arrange
            using var context = new AppDbContext(_options);

            // Add existing tag
            context.Tags.Add(new Tag { Name = "Technology", UserId = Guid.NewGuid() });
            await context.SaveChangesAsync();

            var handler = new AddTagCommandHandler(context);
            var command = new AddTagCommand("Technology", Guid.NewGuid());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(1, await context.Tags.CountAsync()); // Still only 1
        }
    }
}