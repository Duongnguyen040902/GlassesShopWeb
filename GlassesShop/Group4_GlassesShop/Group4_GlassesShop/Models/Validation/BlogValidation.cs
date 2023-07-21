using FluentValidation;
using Group4_GlassesShop.Models.DataModel;

public class BlogValidation : AbstractValidator<BlogModel>
{
    public BlogValidation()
    {
        RuleFor(model => model.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(model => model.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(200).WithMessage("Content must contain at least 200 words.");
        RuleFor(model => model.CategoryBlogId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(model => model.Image)
            .NotEmpty().WithMessage("Image is required.");
    }
}
