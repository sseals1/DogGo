      SELECT o.[Name], o.Email, o.Address, o.NeighborhoodId, n.Name AS NeighborhoodName, o.Phone, d.Name AS DogName
                        FROM Owner o
                        LEFT JOIN Dog d ON d.OwnerId = o.id
                        JOIN Neighborhood n ON n.id = o.NeighborhoodId
                        