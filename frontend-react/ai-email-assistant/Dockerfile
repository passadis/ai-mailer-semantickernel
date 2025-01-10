# Build stage
FROM node:18 AS build

# Set the working directory
WORKDIR /app

# Copy the frontend directory contents into the container at /app
COPY . /app

# Copy the environment file
COPY .env /app/.env

# Install dependencies and build the app
RUN npm install
RUN npm run build

# Serve stage
FROM nginx:alpine


# Copy the build files from the build stage to the Nginx web root directory
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80 for the app
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
