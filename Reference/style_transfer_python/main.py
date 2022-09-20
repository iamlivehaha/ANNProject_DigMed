

import tensorflow as tf
import tensorflow_hub as hub
# image的src绝对路径pip install matplotlib -i
import matplotlib.pyplot as plt
import numpy as np

# 2张图片

# 原始图片
# 读取图片
before_img = plt.imread('./data/Image.jpg')
# before_img = plt.imread('./data/193039f40762337864.jpg')
# before_img = plt.imread('./data/222854636ef1150769.jpg')
# before_img = plt.imread('./data/YellowLabradorLooking_new.jpg')

# img /= 255.
before_img = before_img / 255.

# 图片的显示
# plt.imshow(before_img)
# plt.show()

# 查看图片的数据
# print(before_img)

# 风格图片
# style_img = plt.imread('./data/1.jpg')
style_img = plt.imread('./data/Style.jpg')
# style_img = plt.imread('./data/kadinsky.jpg')
# 归一化，每一个像素点除以255，将其映射到0-1之间
style_img = style_img / 255.
# 图片的显示
# plt.imshow(style_img)
# plt.show()

# 查看图片的数据
# print(style_img)


# 风格迁移
#--------------------------------------------------------------------------------- -
hub_model = hub.load('https://tfhub.dev/google/magenta/arbitrary-image-stylization-v1-256/2')
                      # https://tfhub.dev/google/magenta/arbitrary-image-stylization-v1-256/2
# 把输入规范一下，
# 改变维度
before_img_ = before_img[np.newaxis,:,:,:]
style_img_ = style_img[np.newaxis,:,:,:]


# 传入的是Tensor对象
before_img_ = tf.convert_to_tensor(before_img_,dtype=tf.float32)
style_img_ = tf.convert_to_tensor(style_img_,dtype=tf.float32)




outputs = hub_model(before_img_,style_img_)

# 输出有趣的图片[[[]]]
# print(outputs[0][0])



# 创建子图
plt.subplot(1,3,1)
plt.xlabel('before')
plt.xticks([])
plt.yticks([])
plt.imshow(before_img)

plt.subplot(1,3,2)
plt.xlabel('style')
plt.xticks([])
plt.yticks([])
plt.imshow(style_img)

plt.subplot(1,3,3)
plt.xlabel("after")
plt.xticks([])
plt.yticks([])
plt.imshow(outputs[0][0])


plt.show()



# 图片的保存
X = (outputs[0][0]) * 255
print(X)
# 将X转化为Tensor对象
img = tf.cast(X,dtype=tf.uint8)
# 编码回图片，二进制
img = tf.image.encode_png(img)

print(img)
# 图片保存的路径
save_path = './data/1.jpg'

# 文件的保存
with tf.io.gfile.GFile(save_path,'wb') as file:
    file.write(img.numpy())





