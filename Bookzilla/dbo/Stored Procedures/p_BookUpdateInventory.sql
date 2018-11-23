create procedure p_BookUpdateInventory (
	@BOOK_ID bigint,
	@QUANTITY_IN_STOCK int = 0
) as
begin
	
	if @QUANTITY_IN_STOCK is null
		set @QUANTITY_IN_STOCK = 0

	if exists (select * from Inventory where BOOK_ID = @BOOK_ID)
	begin
		
		update Inventory set
			QUANTITY_IN_STOCK = @QUANTITY_IN_STOCK
		where 
			BOOK_ID = @BOOK_ID

	end
	else
	begin

		insert into Inventory (
			BOOK_ID,
			QUANTITY_IN_STOCK
		) values (
			@BOOK_ID,
			@QUANTITY_IN_STOCK
		)

	end

end