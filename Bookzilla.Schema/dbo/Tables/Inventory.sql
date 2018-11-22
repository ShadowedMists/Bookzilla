﻿CREATE TABLE [dbo].[Inventory] (
    [BOOK_ID]           BIGINT NOT NULL,
    [QUANTITY_IN_STOCK] INT    NOT NULL,
    CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED ([BOOK_ID] ASC),
    CONSTRAINT [CK_Inventory_QUANTITY_IN_STOCK_IS_POSITIVE_OR_ZERO] CHECK ([QUANTITY_IN_STOCK]>=(0)),
    CONSTRAINT [FK_Inventory_Book] FOREIGN KEY ([BOOK_ID]) REFERENCES [dbo].[Book] ([BOOK_ID])
);

