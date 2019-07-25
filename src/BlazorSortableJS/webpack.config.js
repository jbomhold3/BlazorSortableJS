const path = require('path');
var UnminifiedWebpackPlugin = require('unminified-webpack-plugin');

module.exports = {
    entry: './src/ts/BlazorSortableJS.ts',
    output: {
        filename: 'BlazorSortableJs.min.js',
        path: path.resolve(__dirname, 'wwwroot/js')
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/
            }
        ]
    },
    plugins: [
        new UnminifiedWebpackPlugin()
    ]
};