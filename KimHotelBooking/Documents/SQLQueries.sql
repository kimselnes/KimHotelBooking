--Exempel på SELECT query i CustomerReadMenu.Run där jag 'Concatenate':ar variabler/kolumner tillsammans med strängar/text.

SELECT 
    CONCAT('Kundnamn: ', Name, CHAR(10), 
           'Aktiv: ', CASE WHEN IsActive = 1 THEN 'Ja' ELSE 'Nej' END) AS FullStatus
FROM Customer;


-- Exempel på WHERE jag använder i en foreach condition i CustomerService.CustomerListToStringOnlyActive
-- för att filtera bort alla inaktiva (IsActive = false) Customer.

SELECT c.*
FROM Customer c
WHERE c.IsActive = 1;


-- Exempel på ORDER BY som jag använder i CustomerRepository.GetAllCustomers
-- Jag använder ORDER BY varje gång jag hämtar alla entiteter från en tabell i databasen.

SELECT * 
FROM Customer
ORDER BY Id;


-- Exempel på JOIN i InvoiceService.InvoicesToStringList

SELECT 
	b.StartDate, b.EndDate, b.RoomBeds,b.IsActive AS BookingIsActive,
	i.IssueDate, i.DueDate, i.TotalPrice, i.IsPaid, i.IsActive AS InvoiceIsActive,
	r.Id AS RoomId, -- Borde inkluderat RoomNumber eller liknande i tabellen, så Id får ta dess plats...
	c.[Name] AS CustomerName
FROM Invoice i
JOIN Customer c ON i.CustomerId = c.Id
JOIN Booking b ON i.BookingId = b.Id
JOIN Room r ON b.RoomId = r.Id;


-- Exempel på GROUP BY i BookingService.GetBookingsGroupedByRoom

SELECT r.Id AS RoomId, r.[Type] AS RoomType,
COUNT(*) AS NumberOfBookings
FROM Booking b
JOIN Room r ON r.Id = b.RoomId
GROUP BY r.Id, r.[Type];


-- Exempel på SubQuery i CustomerService.CustomersOver2Bookings
-- där jag visar upp alla kunder som har fler än 2 bokningar.

SELECT c.*
FROM Customer c
WHERE (
    SELECT COUNT(b.Id) 
    FROM Booking b WHERE b.CustomerId = c.Id) > 2;