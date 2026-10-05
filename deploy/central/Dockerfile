# The hosted central site: two images from one file (build context: the repository root).
#   target "api": the central API (uvicorn app.central.main:app), Python only
#   target "web": Caddy with the built central page (frontend/dist/central.html) and HTTPS
# docker-compose.yml next to this file builds both.

FROM node:22-alpine AS build-web
WORKDIR /web
COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build

FROM python:3.13-slim AS api
ENV PYTHONDONTWRITEBYTECODE=1 PYTHONUNBUFFERED=1
WORKDIR /app/backend
COPY backend/requirements.txt ./
# pymssql connects to the central SQL Server; pyodbc installs too but stays unused without unixODBC (db.py skips it).
RUN pip install --no-cache-dir -r requirements.txt
COPY backend/app ./app
COPY backend/tools ./tools
COPY backend/migrations ./migrations
RUN useradd --create-home vlqms && mkdir -p data && chown vlqms data
USER vlqms
EXPOSE 8002
# --proxy-headers: Caddy passes the caller's address on, so the sign-in lockouts count per caller.
CMD ["python", "-m", "uvicorn", "app.central.main:app", "--host", "0.0.0.0", "--port", "8002", "--proxy-headers", "--forwarded-allow-ips", "*"]

FROM caddy:2-alpine AS web
COPY --from=build-web /web/dist /srv
# Only the central page: the shop app's page isn't served here.
RUN rm -f /srv/index.html
COPY deploy/central/Caddyfile /etc/caddy/Caddyfile
