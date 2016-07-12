using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using TileSharp.Data.Spatialite;

namespace readShape
{
    class Vertex
    {
        public int id;
        public int idConnexion;
        public double x;
        public double y;

        public void setVertex(int id, int idConnexion)
        {
            this.id = id;
            this.idConnexion = idConnexion;
        }

        public void setVertex(int id, double xCord, double yCord)
        {
            this.id = id;
            this.x = xCord;
            this.y = yCord;
        }
    }

    class Point
    {
        public int id;
        public double x;
        public double y;

        public void setPoint(int id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

    }

    class Edge
    {
        public int id;
        public int nbVoieFT;
        public int nbVoieTF;
        public int vitesse;
        public string hierarchie;
        public string nomRue;
        public List<Point> points;

        public void setEdge(int idArete, int nbVoieFT, int nbVoieTF, int vitesse, string hierarchie, string nomRue)
        {
            this.id = (idArete);
            this.nbVoieFT = (nbVoieFT);
            this.nbVoieTF = (nbVoieTF);
            this.vitesse = (vitesse);
            this.hierarchie = hierarchie;
            this.nomRue = nomRue;
        }
        public void setEdgePoint(int idArete, int nbVoieFT, int nbVoieTF, int vitesse, string hierarchie, string nomRue, List<Point> points)
        {
            this.id = (idArete);
            this.nbVoieFT = (nbVoieFT);
            this.nbVoieTF = (nbVoieTF);
            this.vitesse = (vitesse);
            this.hierarchie = hierarchie;
            this.nomRue = nomRue;
            this.points = points;
        }
    }

    class Flux
    {
        public int id;
        public double angle;
        public int time;
        public int count;
        public int idConnection;

        public void setFlux(int idFlux, double angle, int time, int count, int idConnection)
        {
            this.id = idFlux;
            this.angle = angle;
            this.time = time;
            this.count = count;
            this.idConnection = idConnection;
        }

    }

    class RoadSign
    {
        public int id;
        public string type;
        public double x;
        public double y;
        public double angle;

        public void setRoadSign(int idRoadSign, string type, double x, double y,double angle)
        {
            this.id = idRoadSign;
            this.type = type;
            this.x = x;
            this.y = y;
            this.angle = angle;
        }
    }

    class Bdd
    {
        private SQLiteConnection dbConnection;
        private SQLiteCommand command;
        public Bdd()
        {
            //http://stackoverflow.com/questions/1833640/connection-string-with-relative-path-to-the-database-file
            //dbConnection = new SQLiteConnection("Data Source=bdd.db;Version=3;");
            dbConnection = new SQLiteConnection("Data Source=bddV2.db;Version=3;");
            dbConnection.Open();
        }

        public SQLiteConnection getConnection()
        {
            return dbConnection;
        }

        public void closeConnection()
        {
            //A VOIR !!!
            command.Dispose();
            dbConnection.Close();
        }

        public void deleteAll()
        {
            string sql = "DELETE FROM aretes";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM points";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM connexionSommetArete";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM connexionSommets";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM fluxEntree";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM sommets";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM signalisations";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }

        //Get
        public List<Edge> getAllEdges()
        {
            List<Edge> edges = new List<Edge>();
            string sql = "SELECT * FROM aretes ";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Edge newEdge = new Edge();
                newEdge.setEdge(Convert.ToInt32(reader["idaretes"]), Convert.ToInt32(reader["nbVoieFT"]), Convert.ToInt32(reader["nbVoieTF"]), Convert.ToInt32(reader["Vitesse"]), Convert.ToString(reader["hierarchie"]), Convert.ToString(reader["nomRue"]));
                edges.Add(newEdge);
            }
            return edges;
        }


        public Edge getEdgeById(int idEdge)
        {
            string sql = "SELECT * FROM aretes JOIN points WHERE points.idaretes = @idEdge and aretes.idaretes = @idEdge";
            int nbVoieFT = 0;
            int nbVoieTF = 0;
            int vitesse = 0;
            string hierarchie = "";
            string nomRue = "";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@idEdge", idEdge);
            SQLiteDataReader reader = command.ExecuteReader();
            Edge newEdge = new Edge();
            List<Point> points = new List<Point>();
            while (reader.Read())
            {
                Point point = new Point();
                point.setPoint(Convert.ToInt32(reader["idpoints"]), Convert.ToDouble(reader["x"]), Convert.ToDouble(reader["y"]));
                nbVoieFT = Convert.ToInt32(reader["nbVoieFT"]);
                nbVoieTF = Convert.ToInt32(reader["nbVoieTF"]);
                vitesse = Convert.ToInt32(reader["Vitesse"]);
                hierarchie = Convert.ToString(reader["hierarchie"]);
                nomRue = Convert.ToString(reader["nomRue"]);
                points.Add(point);
            }
            newEdge.setEdgePoint(idEdge, nbVoieFT, nbVoieTF, vitesse, hierarchie, nomRue, points);
            return newEdge;
        }

        public List<Point> getPointsByIdEdge(int idEdge)
        {
            List<Point> newPoints = new List<Point>();
            string sql = "SELECT * FROM points WHERE idaretes = " + idEdge;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Point point = new Point();
                point.setPoint(Convert.ToInt32(reader["idpoints"]), Convert.ToDouble(reader["x"]), Convert.ToDouble(reader["y"]));
                //Console.WriteLine("Point x : " + reader["x"] + " Point y : " + reader["y"]);
                newPoints.Add(point);
            }
            return newPoints;
        }

        public List<Vertex> getAllVertex()
        {
            List<Vertex> vertex = new List<Vertex>();
            string sql = "SELECT * FROM sommets JOIN connexionSommets " +
                "WHERE connexionSommets.idsommet is sommets.idsommet OR connexionSommets.idsommetConnexion is sommets.idsommet";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Vertex newVertex = new Vertex();
                if (Convert.ToInt32(reader[0]) == Convert.ToInt32(reader[2]))
                {
                    newVertex.setVertex(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[3]));
                }
                else
                {
                    newVertex.setVertex(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[2]));
                }
                vertex.Add(newVertex);
                Console.WriteLine("id sommet : " + reader[0] + " Connexion : " + reader[2] + " - " + reader[3]);
            }

            sql = "SELECT sommets.idsommet FROM connexionSommets JOIN sommets " +
                "WHERE connexionSommets.idsommet is not sommets.idsommet and connexionSommets.idsommetConnexion is not sommets.idsommet";
            command = new SQLiteCommand(sql, dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Vertex newVertex = new Vertex();
                newVertex.setVertex(Convert.ToInt32(reader[0]), 0);
                vertex.Add(newVertex);
                Console.WriteLine("id sommet : " + reader[0]);
            }

            return vertex;
        }

        public Vertex getVertex(int idVertex)
        {
            Vertex newVertex = new Vertex();
            string sql = "select * from sommets WHERE idsommet is " + idVertex;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("id sommet : " + reader["idsommet"]);

                newVertex.setVertex(Convert.ToInt32(reader[0]), 0);
            }
            return newVertex;
        }

        public Vertex getVertexConnByVertex(int idVertex)
        {
            Vertex newVertex = new Vertex();
            string sql = "select * from sommets JOIN connexionSommets " +
                "WHERE(connexionSommets.idsommet is " + idVertex + " OR connexionSommets.idsommetConnexion is " + idVertex + ") AND sommets.idsommet is " + idVertex;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("id sommet : " + reader["idsommet"] + " Connexion : " + reader["idsommet"] + " - " + reader["idsommetConnexion"]);
                if (Convert.ToInt32(reader[0]) == Convert.ToInt32(reader[2]))
                {
                    newVertex.setVertex(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[3]));
                }
                else
                {
                    newVertex.setVertex(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[2]));
                }
            }
            return newVertex;
        }

        /*public Vertex getVextexConnByEdge(int idEdge) {
            Vertex newVertex = new Vertex();
            string sql = "select * from sommets JOIN connexionSommets " +
                "WHERE(connexionSommets.idsommet is " + idEdge + " OR connexionSommets.idsommetConnexion is " + idEdge + ") AND sommets.idsommet is " + idEdge;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) { 
                Console.WriteLine("id sommet : " + reader["idsommet"] + " debut : " + reader["debut"] + " Connexion : " + reader["idsommet"] + " - " + reader["idsommetConnexion"]);
                if (Convert.ToInt32(reader[0]) == Convert.ToInt32(reader[2]))
                {
                    newVertex.setVertex(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), Convert.ToInt32(reader[3]));
                }
                else
                {
                    newVertex.setVertex(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), Convert.ToInt32(reader[2]));
                }
            }
            return newVertex;
        }*/
        public int getIdVertexByEdge(int idEdge)
        {
            string sql = "select connexionSommetArete.idsommet from aretes JOIN connexionSommetArete  " +
                "WHERE connexionSommetArete.idaretes is " + idEdge + " AND aretes.idaretes is " + idEdge;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
                return Convert.ToInt32(reader["idaretes"]);
            return -1;
        }

        public int getIdEdgeByVertex(int idVertex)
        {
            string sql = "select connexionSommetArete.idaretes from sommets JOIN connexionSommetArete  " +
                "WHERE connexionSommetArete.idsommet is " + idVertex + " AND sommets.idsommet is " + idVertex;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
                return Convert.ToInt32(reader["idaretes"]);
            return -1;
        }

        public List<Edge> getEdgeByVertex(int idVertex)
        {
            List<Edge> edges = new List<Edge>();
            string sql = "SELECT aretes.idaretes,aretes.nbVoieFT, aretes.nbVoieTF, aretes.Vitesse, aretes.hierarchie, aretes.nomRue FROM aretes " +
                "JOIN connexionSommetArete ON connexionSommetArete.idaretes = aretes.idaretes " +
                "AND connexionSommetArete.idsommet = " + idVertex;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Edge newEdge = new Edge();
                Console.WriteLine("idArete : " + reader["idaretes"] + " NbVoieFT/TF : " + reader["nbVoieFT"] + "/" + reader["nbVoieTF"] + " Vitesse : " + reader["Vitesse"]);
                newEdge.setEdge(Convert.ToInt32(reader["idaretes"]), Convert.ToInt32(reader["nbVoieFT"]), Convert.ToInt32(reader["nbVoieTF"]), Convert.ToInt32(reader["Vitesse"]), Convert.ToString(reader["hierarchie"]), Convert.ToString(reader["nomRue"]));
                edges.Add(newEdge);
            }
            return edges;
        }

        public List<Vertex> getVertexByEdge(int idEdge)
        {
            List<Vertex> vertex = new List<Vertex>();
            string sql = "SELECT sommets.idsommet FROM sommets " +
                "JOIN connexionSommetArete ON connexionSommetArete.idsommet = sommets.idsommet " +
                "AND connexionSommetArete.idaretes = " + idEdge;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Vertex newVertex = new Vertex();
                Console.WriteLine("idSommet : " + reader["idsommet"] + " debut : " + reader["debut"]);
                newVertex.setVertex(Convert.ToInt32(reader[0]), 0);
                vertex.Add(newVertex);
            }
            return vertex;
        }

        public Flux getFluxByEdge(int idEdge)
        {
            Flux flux = new Flux();
            string sql = "SELECT * FROM aretes JOIN fluxEntree " +
                "WHERE fluxEntree.idaretes is " + idEdge + " AND aretes.idaretes is " + idEdge;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("idEdge : " + reader["idaretes"] + " idfluxEntre : " + reader["idfluxEntre"] + "angle / time / count" + reader["angle"] + reader["time"] + reader["count"]);
                flux.setFlux(Convert.ToInt32(reader["idfluxEntre"]), Convert.ToDouble(reader["angle"]), Convert.ToInt32(reader["time"]), Convert.ToInt32(reader["count"]), Convert.ToInt32(reader["idaretes"]));

            }
            return flux;
        }

        public List<Flux> getAllFlux()
        {
            List<Flux> flux = new List<Flux>();
            string sql = "SELECT * FROM fluxEntree";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Flux newFlux = new Flux();
                newFlux.setFlux(Convert.ToInt32(reader["idfluxEntre"]), Convert.ToDouble(reader["angle"]), Convert.ToInt32(reader["time"]), Convert.ToInt32(reader["count"]), Convert.ToInt32(reader["idaretes"]));
                Console.WriteLine("idSommet : " + reader["idsommet"] + " idfluxEntre : " + reader["idfluxEntre"] + "angle / time / count" + reader["angle"] + reader["time"] + reader["count"]);
                flux.Add(newFlux);
            }
            return flux;
        }

        /*public int getIdVertexByPoints(double x, double y)
        {
            int idVertex = 0;
            string sql = "SELECT connexionSommetArete.idsommet FROM aretes " +
                        "JOIN points ON(aretes.idaretes = points.idaretes) " +
                        "JOIN connexionSommetArete ON(aretes.idaretes = connexionSommetArete.idaretes) " +
                        "WHERE points.x LIKE " + x + " AND points.y LIKE " + y;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                idVertex = Convert.ToInt32(reader[0]);
            }
            return idVertex;
        }*/
        

        public List<int> getConnexionByVertex(int idVertex)
        {
            List<int> lIdVertex = new List<int>();
            string sql = "SELECT * FROM connexionSommets " +
                "WHERE idsommet LIKE " + idVertex +
                " OR idsommetConnexion LIKE " + idVertex;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (idVertex != (Convert.ToInt32(reader[0])))
                {
                    lIdVertex.Add(Convert.ToInt32(reader[0]));
                } else if (idVertex != (Convert.ToInt32(reader[1])))
                {
                    lIdVertex.Add(Convert.ToInt32(reader[1]));
                }
            }
            return lIdVertex;
        }

        public Point getFirstPointsByEdge(int idEdge)
        {
            Point point = new Point();
            string sql = "SELECT points.x, points.y FROM aretes JOIN points ON aretes.idaretes = points.idaretes " +
                "WHERE aretes.idaretes LIKE " + idEdge + " LIMIT 1";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                point.x = Convert.ToDouble(reader[0]);
                point.y = Convert.ToDouble(reader[1]);
            }
            return point;
        }

        public Point getLastPointsByEdge(int idEdge)
        {
            Point point = new Point();
            string sql = "SELECT points.x, points.y FROM aretes JOIN points ON aretes.idaretes = points.idaretes " + 
                "WHERE aretes.idaretes LIKE " + idEdge + " ORDER BY points.idpoints DESC LIMIT 1";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                point.x = Convert.ToDouble(reader[0]);
                point.y = Convert.ToDouble(reader[1]);
            }
            return point;
        }

        public Point getPointOfVertex(int idVertex)
        {
            Point point = new Point();
            string sql = "SELECT sommets.posX, sommets.posY FROM sommets " +
                "WHERE sommets.idsommet = " + idVertex;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                point.x = Convert.ToDouble(reader[0]);
                point.y = Convert.ToDouble(reader[1]);
            }
            return point;
        }

        public int getIdVertexByPoint(double x, double y)
        {
            int id = -1;
            string sql = "SELECT sommets.idsommet FROM sommets " +
               "WHERE sommets.posX LIKE " + x + " AND sommets.posY LIKE " + y;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = Convert.ToInt32(reader[0]);
            }
            return id;
        }

        public int getSpeedByIdEdge(int idEdge)
        {
            int speed = 0;
            string sql = "SELECT Vitesse FROM aretes " +
                "WHERE idaretes = " + idEdge;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                speed = Convert.ToInt32(reader[0]);
            }
            return speed;
        }

        public List<Vertex> getAllVertexOut() {
            List<Vertex> vertex = new List<Vertex>();
            string sql = "SELECT * FROM sommets " + 
                "WHERE sommets.posY > 1117008 OR sommets.posX < 2500228 OR sommets.posX > 2501121 OR sommets.posY < 1116112";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Vertex newVertex = new Vertex();
                newVertex.setVertex(Convert.ToInt32(reader[0]), Convert.ToDouble(reader[1]), Convert.ToDouble(reader[2]));
                vertex.Add(newVertex);
            }
            return vertex;
        }

        public List<int> getAllIdEdgeOut(double xHaut, double yGauche, double xBas, double yDroite)
        {

            List<int> lOut = new List<int>();
            string sql = "SELECT idaretes FROM points " +
                "WHERE(y > " + yGauche + " OR x < " + xHaut + " OR x > " + xBas + " OR y < " + yDroite + ") " +
                "GROUP BY idaretes";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idAretes = Convert.ToInt32(reader[0]);
                lOut.Add(idAretes);
            }
            return lOut;
        }

        public List<Edge> getAllEdgesOut()
        {
            List<Edge> edges = new List<Edge>();
            string sql = "SELECT aretes.idaretes, aretes.nbVoieFT, aretes.nbVoieTF, aretes.Vitesse,aretes.hierarchie, aretes.nomRue FROM sommets " +
                " JOIN connexionSommetArete ON connexionSommetArete.idsommet = sommets.idsommet" +
                " JOIN aretes ON aretes.idaretes = connexionSommetArete.idaretes" +
                " WHERE sommets.posY > 1117008 OR sommets.posX < 2500228 OR sommets.posX > 2501121 OR sommets.posY < 1116112" +
                " GROUP BY aretes.idaretes";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Edge newEdge = new Edge();
                newEdge.setEdge(Convert.ToInt32(reader["idaretes"]), Convert.ToInt32(reader["nbVoieFT"]), Convert.ToInt32(reader["nbVoieTF"]), Convert.ToInt32(reader["Vitesse"]), Convert.ToString(reader["hierarchie"]), Convert.ToString(reader["nomRue"]));
                edges.Add(newEdge);
            }
            return edges;
        }

        public List<RoadSign> getAllRoadSign()
        {
            List<RoadSign> roadSigns = new List<RoadSign>();
            string sql = "SELECT * FROM signalisations";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                RoadSign newRoadSign = new RoadSign();
                newRoadSign.setRoadSign(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), Convert.ToDouble(reader[2]), Convert.ToDouble(reader[3]), Convert.ToDouble(reader[4]));
                roadSigns.Add(newRoadSign);
            }
            return roadSigns;
        }

  /*      public Point getFirstPointByVertex(int idVertex)
        {
            "SELECT * FROM connexionSommetArete
JOIN aretes ON aretes.idaretes = connexionSommetArete.idaretes
JOIN points ON aretes.idaretes = points.idaretes
WHERE connexionSommetArete.idsommet LIKE 370
ORDER BY points.idpoints DESC
LIMIT 1"
        }
        */
        //Insertion
        public void insertPointsEdge(double x, double y, int idEdge)
        {
            string sql = "INSERT INTO points (x, y, idaretes) values (@x, @y, @idEdge)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@x", x);
            command.Parameters.AddWithValue("@y", y);
            command.Parameters.AddWithValue("@idEdge", idEdge);
            command.ExecuteNonQuery();
        }

        public int insertEdges(int nbVoieFT, int nbVoieTF, int vitesse, string hierarchie, string nomRue)
        {
            string sql = "INSERT INTO aretes (nbVoieFT, nbVoieTF, Vitesse, hierarchie, nomRue) values (@nbVoieFT, @nbVoieTF, @vitesse, @hierarchie, @nomRue)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@nbVoieFT", nbVoieFT);
            command.Parameters.AddWithValue("@nbVoieTF", nbVoieTF);
            command.Parameters.AddWithValue("@hierarchie", hierarchie);
            command.Parameters.AddWithValue("@nomRue", nomRue);
            if (vitesse < 0)
            {
                command.Parameters.AddWithValue("@vitesse", vitesse);
            }
            else
            {
                command.Parameters.AddWithValue("@vitesse", 50);
            }
            command.ExecuteNonQuery();
            return (int)dbConnection.LastInsertRowId;
        }

        public int insertVertexByEdge(int idEdge,  double x, double y)
        {
            int idVertex = 0;
            string sql = "INSERT INTO sommets( posX, posY) values (@x, @y)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@x", x);
            command.Parameters.AddWithValue("@y", y);
            command.ExecuteNonQuery();
            idVertex = (int)dbConnection.LastInsertRowId;
            sql = "INSERT INTO connexionSommetArete(idsommet, idaretes) VALUES (@idsommet, @idedge)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@idsommet", (int)dbConnection.LastInsertRowId);
            command.Parameters.AddWithValue("@idedge", idEdge);
            command.ExecuteNonQuery();
            return idVertex;
        }

        public void insertConnexionVertexEdge(int idEdge, int idVertex)
        {
            string sql = "INSERT INTO connexionSommetArete(idsommet, idaretes) VALUES (@idsommet, @idedge)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@idsommet", idVertex);
            command.Parameters.AddWithValue("@idedge", idEdge);
            command.ExecuteNonQuery();
        }

        public void insertConnexionVertex(int idVertex, int idVertexConnexion)
        {
            string sql = "SELECT * FROM connexionSommets WHERE idsommet = "+ idVertex +" AND idsommetConnexion = " + idVertexConnexion;
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                return;
            }
            /*sql = "SELECT * FROM connexionSommets WHERE idsommet = " + idVertexConnexion + " AND idsommetConnexion = " + idVertex;
            command = new SQLiteCommand(sql, dbConnection);
            reader = command.ExecuteReader();
            if (!reader.Read())
            {
                Console.WriteLine("DEJA EXISTANT : " + idVertex + " " + idVertexConnexion);
            } */

            sql = "INSERT INTO connexionSommets(idsommet, idsommetConnexion) values (@idvertex, @idvertexConnexion)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@idvertex", idVertex);
            command.Parameters.AddWithValue("@idvertexConnexion", idVertexConnexion);
            command.ExecuteNonQuery();
        }

        public void insertFluxEntree(int countFT, int countTF, int idaretes)
        {
            string sql = "INSERT INTO fluxEntree(countFT, countTF, idaretes) values (@countFT, @countTF, @idaretes)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@countFT", countFT);
            command.Parameters.AddWithValue("@countTF", countTF);
            command.Parameters.AddWithValue("@idaretes", idaretes);
            command.ExecuteNonQuery();
        }

        public void insertFlux(int countFT, int countTF, int idaretes)
        {
            string sql = "INSERT INTO fluxEntree(countFT, countTF, idaretes) values (@countFT, @countTF, @idaretes)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@countFT", countFT);
            command.Parameters.AddWithValue("@countTF", countTF);
            command.Parameters.AddWithValue("@idaretes", idaretes);
            command.ExecuteNonQuery();
        }
        

        public void insertSignalisation(string type, double x, double y, double angle)
        {
            string sql = "INSERT INTO signalisations(type, x, y, angle) values (@type, @x, @y, @angle)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@x", x);
            command.Parameters.AddWithValue("@y", y);
            command.Parameters.AddWithValue("@angle", angle);
            command.ExecuteNonQuery();
        }

        public void insertFluxEntry(int time, int count, int idVertex)
        {
            string sql = "INSERT INTO fluxEntree(time, count, idsommet) values (@time, @count, @idsommet)";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@time", time);
            command.Parameters.AddWithValue("@count", count);
            command.Parameters.AddWithValue("@idsommet", idVertex);
        }

        public void insertUpdateFluxEntry(double angle, int time, int count, int idEdge)
        {
            string sql = "insert or replace into fluxEntree (idfluxEntre, angle, time, count, idaretes) values ( " + 
               "(select fluxEntree.idfluxEntre from fluxEntree where fluxEntree.idaretes = @idEdge), " +
               "@angle, @time, @count, @idEdge); ";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@angle", angle);
            command.Parameters.AddWithValue("@time", time);
            command.Parameters.AddWithValue("@count", count);
            command.Parameters.AddWithValue("@idEdge", idEdge);
            command.ExecuteNonQuery();
        }

        public void updateVitesseByIdEdge(int idEdge, int vitesse)
        {
            string sql = "UPDATE aretes SET Vitesse = @vitesse WHERE idaretes = @idedge";
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.AddWithValue("@vitesse", vitesse);
            command.Parameters.AddWithValue("@idedge", idEdge);
            command.ExecuteNonQuery();
        }
        public bool emptyBdd()
        {
            if (!emptyRoadGraph())
                return false;
            if (!emptyFlux())
                return false;
            if (!emptyRoadSign())
                return false;
            if (!emptySpeed())
                return false;
            return true;
        }
        public bool emptyRoadGraph()
        {
            //http://stackoverflow.com/questions/27373344/sqlite-database-gives-warning-automatic-index-on-table-namecolumn-after-upgr
            //Message d'information pour enlever l'auto index
            int count = 0;
            string sql = "SELECT COUNT(*) FROM sommets" + 
              " JOIN connexionSommetArete ON sommets.idsommet IS connexionSommetArete.idsommet" +
              " JOIN connexionSommets ON sommets.idsommet IS connexionSommets.idsommet OR sommets.idsommet IS connexionSommets.idsommetConnexion" +
              " JOIN aretes ON aretes.idaretes IS connexionSommetArete.idaretes" +
              " JOIN points ON aretes.idaretes IS points.idaretes";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                count = Convert.ToInt32(reader[0]);
            }
            if (count > 0)
                return false;
            return true;
        }

        public bool emptySpeed()
        {
            int count = 0;
            string sql = "SELECT COUNT(aretes.Vitesse) FROM aretes";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                count = Convert.ToInt32(reader[0]);
            }
            if (count > 0)
                return false;
            return true;
        }
        public bool emptyRoadSign()
        {
            int count = 0;
            string sql = "SELECT COUNT(*) FROM signalisations";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                count = Convert.ToInt32(reader[0]);
            }
            if (count > 0)
                return false;
            return true;
        }
        public bool emptyFlux()
        {
            int count = 0;
            string sql = "SELECT COUNT(*) FROM fluxEntree";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                count = Convert.ToInt32(reader[0]);
            }
            if (count > 0)
                return false;
            return true;
        }

        public void deleteRoadGraph()
        {
            string sql = "DELETE FROM aretes";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM points";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM connexionSommetArete";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM connexionSommets";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            sql = "DELETE FROM sommets";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }

        public void deleteRoadSign()
        {
            string sql = "DELETE FROM signalisations";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }

        public void deleteFlux()
        {
            string sql = "DELETE FROM fluxEntree";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }

        public void deleteSpeed()
        {
            string sql = "UPDATE aretes SET Vitesse = 50";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }
    }
}
