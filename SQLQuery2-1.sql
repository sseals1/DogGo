SELECT w.Id, w.Date, w.Duration, o.Name AS Client FROM Walks w
JOIN Dog d ON d.Id = w.DogId
JOIN Owner o ON o.Id = d.OwnerId

SELECT * FROM Walks w
JOIN Dog d ON d.Id = w.DogId
JOIN Owner o ON o.Id = d.OwnerId

 