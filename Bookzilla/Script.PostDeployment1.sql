set identity_insert dbo.Author on
GO

merge into [dbo].[Author] as Target
using(values
	(1, 'J. R. R. Tolkien'),
	(2, 'Phillip K. Dick'),
	(3, 'Don Norman'),
	(4, 'Cormac McCarthy'),
	(5, 'Friedrich Nietzsche')
)
as Source ([AUTHOR_ID], [AUTHOR_DISPLAY_NAME])
on 
	Target.[AUTHOR_ID] = Source.[AUTHOR_ID]
when matched then
update set 
	Target.[AUTHOR_DISPLAY_NAME] = Source.[AUTHOR_DISPLAY_NAME]
when not matched by target then
insert ([AUTHOR_ID], [AUTHOR_DISPLAY_NAME])
values ([AUTHOR_ID], [AUTHOR_DISPLAY_NAME])
when not matched by source then delete;
GO

set identity_insert dbo.Author off
GO

set identity_insert dbo.Book on
GO

merge into [dbo].[Book] as Target
using(values
	(1, 1, 'The Hobbit', 'A very small person goes on an unexpected adventure.', 276, '1/1/1937 12:00:00 AM', '9780547928241'),
	(2, 2, 'Do Androids Dream of Electric Sheep?', 'Let''s go kill some robots with my robot girlfriend.', 224, '1/1/1968 12:00:00 AM', '9780345404473'),
	(3, 3, 'The Design of Everyday Things (Revised Edition)', 'Doors are hard. Software shouldn''t be.', 298, '1/1/2013 12:00:00 AM', '9780465050659'),
	(4, 4, 'The Road', 'Everybody dies but why am I still crying?', 287, '1/1/2006 12:00:00 AM', '9780397387899'),
	(5, 5, 'Beyond Good & Evil', 'Self-described poet-philosopher lets you know in 245 pages that his way is better than your way; like he read Marcus Aurelius''s Meditations and thought he could do better.', 245, '1/1/1966 12:00:00 AM', '9780679724650')
)
as Source ([BOOK_ID], [AUTHOR_ID], [BOOK_TITLE], [BOOK_SUMMARY], [BOOK_TOTAL_PAGES], [BOOK_RELEASE_DATE], [BOOK_ISBN])
on 
	Target.[BOOK_ID] = Source.[BOOK_ID]
when matched then
update set 
	Target.[AUTHOR_ID] = Source.[AUTHOR_ID],
	Target.[BOOK_TITLE] = Source.[BOOK_TITLE],
	Target.[BOOK_SUMMARY] = Source.[BOOK_SUMMARY],
	Target.[BOOK_TOTAL_PAGES] = Source.[BOOK_TOTAL_PAGES],
	Target.[BOOK_RELEASE_DATE] = Source.[BOOK_RELEASE_DATE],
	Target.[BOOK_ISBN] = Source.[BOOK_ISBN]
when not matched by target then
insert ([BOOK_ID], [AUTHOR_ID], [BOOK_TITLE], [BOOK_SUMMARY], [BOOK_TOTAL_PAGES], [BOOK_RELEASE_DATE], [BOOK_ISBN])
values ([BOOK_ID], [AUTHOR_ID], [BOOK_TITLE], [BOOK_SUMMARY], [BOOK_TOTAL_PAGES], [BOOK_RELEASE_DATE], [BOOK_ISBN])
when not matched by source then delete;
GO

set identity_insert dbo.Book off
GO

merge into [dbo].[Inventory] as Target
using(values
	(1, 3),
	(2, 5),
	(3, 2),
	(4, 0),
	(5, 4)
)
as Source ([BOOK_ID], [QUANTITY_IN_STOCK])
on 
	Target.[BOOK_ID] = Source.[BOOK_ID]
when matched then
update set 
	Target.[QUANTITY_IN_STOCK] = Source.[QUANTITY_IN_STOCK]
when not matched by target then
insert ([BOOK_ID], [QUANTITY_IN_STOCK])
values ([BOOK_ID], [QUANTITY_IN_STOCK])
when not matched by source then delete;
GO
