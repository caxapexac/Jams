const ASSET_NAMES = [
    'ship.png',
    'ship_charge.png',
    'bullet.svg',
    'spike.png',
    'star2.png'
]

const assets = {}

function downloadAsset (assetName) {
    return new Promise((resolve) => {
        const asset = new Image()
        asset.onload = () => {
            console.log(`Downloaded ${assetName}`)
            assets[assetName] = asset
            resolve()
        }
        asset.src = `/assets/${assetName}`
    })
}

const downloadPromise = Promise.all(ASSET_NAMES.map(downloadAsset))

const downloadAssets = () => downloadPromise

const getAsset = assetName => assets[assetName]

export default {
    downloadAssets,
    getAsset
}
