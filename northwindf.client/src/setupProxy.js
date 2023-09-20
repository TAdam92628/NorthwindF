const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:34681';

const context = [
    "/weatherforecast",
];

module.exports = function (app) {
    //const appProxy = createProxyMiddleware(context, {
    //    target: 'https://localhost:5001',
    //    secure: false
    //});
const appProxy = createProxyMiddleware(context, {
        target: 'http://localhost:5064/',
        secure: false,
        headers: {
            Connection: 'Keep-Alive'
        }
    });
    app.use(appProxy);
};
