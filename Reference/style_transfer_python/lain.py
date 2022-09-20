
import tensorflow as tf
import tensorflow_hub as hub
import matplotlib.pyplot as plt
import sys
import numpy as np

import uuid

# 风格迁移
def get_image(image_path,style_path):



    # 原图
    img = plt.imread(image_path)
    img = img/255.

    # 风格图
    style = plt.imread(style_path)
    style = style/255.

    # 风格迁移模型
    hub_handle = "https://tfhub.dev/google/magenta/arbitrary-image-stylization-v1-256/2"
    hub_model = hub.load(hub_handle)

    # 把输入改为符合别人模型的输入
    content_img = img[np.newaxis,:,:,:]
    content_img = tf.convert_to_tensor(content_img,dtype=tf.float32)

    style_image = style[np.newaxis,:,:,:]
    style_image = tf.convert_to_tensor(style_image,dtype=tf.float32)

    # 生成风格迁移的图片
    outputs = hub_model(content_img,style_image)

    # 把生成的图片保存下来
    x = (outputs[0][0]) * 255
    img = tf.cast(x,dtype=tf.uint8)
    img = tf.image.encode_png(img)

    save_path = "D:\\Project\\siki_python\\style_transfer\\image\\"
    # 图片的绝对路径
    save_name = str(uuid.uuid4()) + '.jpg'

    # 文件的保存
    with tf.io.gfile.GFile(save_path + save_name,'wb') as file:
        file.write(img.numpy())


    return save_name


print(get_image(sys.argv[1],sys.argv[2]))








