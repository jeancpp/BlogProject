SELECT *
  FROM [BlogDb].[dbo].[BlogPost]

  SELECT *
  FROM [BlogDb].[dbo].BlogPostTag

  SELECT b.*, t.Name
  FROM [BlogDb].[dbo].[BlogPost] b
  join dbo.BlogPostTag bt on b.Id = bt.BlogPostId
  join dbo.Tag t on t.Id = bt.TagsId