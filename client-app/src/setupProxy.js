const { createProxyMiddleware } = require("http-proxy-middleware");

module.exports = function (app) {
	app.use(
		"/api/NumberToWords/convert?number",
		createProxyMiddleware({
			target: "http://localhost:7138", // Port where the .NET Web API is running
			changeOrigin: true,
		})
	);
};
