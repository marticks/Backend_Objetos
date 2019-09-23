﻿using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Domain;
using WebApiObjetos.Models.Entities;
using WebApiObjetos.Models.Repositories.Interfaces;
using WebApiObjetos.Services.Interfaces;

namespace WebApiObjetos.Services
{
    public class LocationService : ILocationService
    {
        private ILocationRepository locationRepo;
        private IImageRepository imageRepo;

        public LocationService(ILocationRepository locationRepo, IImageRepository imageRepo)
        {
            this.locationRepo = locationRepo;
            this.imageRepo = imageRepo;
        }


        public async Task<bool> DeleteLocation(int userId, int locationId)
        {
            return await locationRepo.deleteLocation(userId, locationId);
        }

        public async Task<LocationDTO> GetLocation(int userId, int locationId)
        {
            var result = await locationRepo.FindBy(x => x.UserId.Equals(userId) && x.Id.Equals(locationId));
            if (result.Count > 0)
                return result.First().toDto();
            return null;
        }


        public async Task<List<LocationDTO>> GetLocations(int userId)
        {
            var locations = await locationRepo.FindBy(x => x.UserId.Equals(userId));
            List<LocationDTO> locationsDTO = new List<LocationDTO>();
            foreach (Location location in locations)
            {
                locationsDTO.Add(location.toDto());
            }
            return locationsDTO;
        }

        public async Task<LocationDTO> AddLocation(LocationDTO location)
        {
            try
            {
                if (location.ImageId == 0)
                    location.ImageId = null;
                return (await locationRepo.Add(location.ToEntity())).toDto();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateLocation(LocationDTO location)
        {
            var exists = await locationRepo.Any(x => x.Id.Equals(location.Id) && x.UserId.Equals(location.UserId));
            if (exists)
            {
                if (location.ImageId == 0)
                    location.ImageId = null;
                await locationRepo.Update(location.ToEntity());
                return true;
            }
            return false;
        }

        public async Task<ImageDTO> AddImage(ImageDTO image)
        {
            try
            {
                return (await imageRepo.Add(image.ToEntity())).toDto();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ImageDTO> GetImage(int imageId, int userId)
        {
            var result = await imageRepo.FindBy(x => x.Id.Equals(imageId) && x.UserId.Equals(userId));

            if (result.Count > 0)
                return result.First().toDto();
            return null;
        }


        public async Task<List<LocationDTO>> getLocationsInArea(string coordinates, int userId)
        {
            var userLocations = await this.GetLocations(userId);

            List<GeoCoordinate> AreaCoordinates = getPoints(coordinates);

            List<LocationDTO> returnList = new List<LocationDTO>();

            foreach (LocationDTO location in userLocations)
            {
                var isIn = true;
                List<GeoCoordinate> locationCoordinates = getPoints(location.Coordinates);
                int i = 0;
                while (i < locationCoordinates.Count && isIn)
                {
                    var point = locationCoordinates[i];
                    isIn = IsInPolygon(AreaCoordinates, point);
                    i++;
                }
                if (isIn)
                    returnList.Add(location);
            }
            return returnList;
        }

        //x=longitude
        //y=latitude
        // Return True if the point is in the polygon.
        private bool IsInPolygon(List<GeoCoordinate> AreaCoordinates, GeoCoordinate point)
        {
            // Get the angle between the point and the
            // first and last vertices.
            int max_point = AreaCoordinates.Count - 1;
            double total_angle = GetAngle(
                AreaCoordinates[max_point].Longitude, AreaCoordinates[max_point].Latitude,
                point.Longitude, point.Latitude,
                AreaCoordinates[0].Longitude, AreaCoordinates[0].Latitude);

            for (int i = 0; i < max_point; i++)
            {
                total_angle += GetAngle(
                    AreaCoordinates[i].Longitude, AreaCoordinates[i].Latitude,
                    point.Longitude, point.Latitude,
                    AreaCoordinates[i + 1].Longitude, AreaCoordinates[i + 1].Latitude);
            }

            return (Math.Abs(total_angle) > 1);
        }


        private static double GetAngle(double Ax, double Ay,
            double Bx, double By, double Cx, double Cy)
        {
            double dot_product = DotProduct(Ax, Ay, Bx, By, Cx, Cy);

            double cross_product = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy);

            return (double)Math.Atan2(cross_product, dot_product);
        }

        private static double DotProduct(double Ax, double Ay,
            double Bx, double By, double Cx, double Cy)
        {
            // Get the vectors' coordinates.
            double BAx = Ax - Bx;
            double BAy = Ay - By;
            double BCx = Cx - Bx;
            double BCy = Cy - By;

            return (BAx * BCx + BAy * BCy);
        }

        private static double CrossProductLength(double Ax, double Ay,
            double Bx, double By, double Cx, double Cy)
        {
            // Get the vectors' coordinates.
            double BAx = Ax - Bx;
            double BAy = Ay - By;
            double BCx = Cx - Bx;
            double BCy = Cy - By;

            return (BAx * BCy - BAy * BCx);
        }

        private List<GeoCoordinate> getPoints(String locationsString)
        {
            List<GeoCoordinate> coordinatesList = new List<GeoCoordinate>();

            foreach (String location in locationsString.Split(';'))
            {
                if (!location.Equals(""))
                {
                    String lat = location.Split(",")[0];
                    String lng = location.Split(",")[1];
                    double a = Double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);
                    double b = Double.Parse(lng, System.Globalization.CultureInfo.InvariantCulture);
                    GeoCoordinate latLng = new GeoCoordinate();
                    latLng = new GeoCoordinate(a, b);
                    coordinatesList.Add(latLng);
                }
            }
            return coordinatesList;
        }
    }
}
