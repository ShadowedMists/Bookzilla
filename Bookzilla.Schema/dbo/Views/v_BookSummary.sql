
create view v_BookSummary as
select
	b.BOOK_ID,
	b.BOOK_TITLE,
	b.AUTHOR_ID,
	a.AUTHOR_DISPLAY_NAME,
	b.BOOK_RELEASE_DATE,
	b.BOOK_ISBN
from Book b
inner join Author a on b.AUTHOR_ID = a.AUTHOR_ID