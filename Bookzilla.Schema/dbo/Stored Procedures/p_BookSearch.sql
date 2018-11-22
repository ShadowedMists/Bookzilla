create procedure p_BookSearch (
	@SEARCH_TEXT nvarchar(256)
) as
begin

	select
		b.BOOK_ID,
		b.AUTHOR_ID,
		a.AUTHOR_DISPLAY_NAME,
		b.BOOK_TITLE,
		b.BOOK_SUMMARY,
		b.BOOK_TOTAL_PAGES,
		b.BOOK_RELEASE_DATE,
		b.BOOK_ISBN,
		isnull(i.QUANTITY_IN_STOCK, 0) QUANTITY_IN_STOCK
	from Book b
	inner join Author a on b.AUTHOR_ID = a.AUTHOR_ID
	left join Inventory i on b.BOOK_ID = i.BOOK_ID
	where
		@SEARCH_TEXT is null
		or b.BOOK_TITLE like '%' + @SEARCH_TEXT + '%'
		or a.AUTHOR_DISPLAY_NAME like '%' + @SEARCH_TEXT + '%'
	order by b.BOOK_TITLE

end