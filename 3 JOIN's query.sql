SELECT w.Id, w.Date, w.Duration, o.Name, d.ImageUrl, o.NeighborhoodId, n.Name FROM Walks w
                       JOIN Dog d ON d.Id = w.DogId
                       JOIN Owner o ON o.Id = d.OwnerId
                       JOIN Neighborhood n ON n.Id = o.NeighborhoodId