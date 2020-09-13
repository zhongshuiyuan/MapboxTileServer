const path = require('path');

module.exports = {
  // publicPath is configured to use a relative URI path during production
  // so the viewer works when behind a proxy
  publicPath: process.env.NODE_ENV === 'production' ? '.' : '/',
  chainWebpack: (config) => {
    config.resolve.alias.set('@', path.resolve(__dirname, 'src'));
  }
};
